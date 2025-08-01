using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;

public class GoapPlanner
{

    public event Action<IEnumerable<GOAPAction>> OnPlanCompleted;
    public event Action OnCantPlan;

    private const int _WATCHDOG_MAX = 200;

    private int _watchdog;

    public void Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions,
                    Func<IEnumerator, Coroutine> startCoroutine)
    {
        _watchdog = _WATCHDOG_MAX;

        var astar = new AStar<GOAPState>();
        astar.OnPathCompleted += OnPathCompleted;
        astar.OnCantCalculate += OnCantCalculate;

        var astarEnumerator = astar.Run(from,
                                        state => Satisfies(state, to),
                                        node => Explode(node, actions, ref _watchdog),
                                        state => GetHeuristic(state, to));

        startCoroutine(astarEnumerator);
    }

    //public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine)
    //{
    //    var prevState = plan.First().linkedState;

    //    var fsm = new FiniteStateMachine(prevState, startCoroutine);

    //    foreach (var action in plan.Skip(1))
    //    {
    //        if (prevState == action.linkedState) continue;
    //        fsm.AddTransition("On" + action.linkedState.Name, prevState, action.linkedState);

    //        prevState = action.linkedState;
    //    }

    //    return fsm;
    //}

    //GOAP
    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine)
    {
        var actions = plan.ToList();
        if (actions.Count == 0) return null;

        var fsm = new FiniteStateMachine(actions[0].linkedState, startCoroutine);

        var configuredStates = new List<IState>();
        foreach (var action in actions)
        {
            var state = action.linkedState.Configure(fsm);
            configuredStates.Add(state);
        }

        for (int i = 0; i < actions.Count - 1; i++)
        {
            var currentState = actions[i].linkedState;
            var nextState = actions[i + 1].linkedState;

            string transitionKey = $"GoapStep{i}";

            fsm.AddTransition(transitionKey, currentState, nextState);

            //Callback al terminar estado
            if (currentState is MonoBaseState monoState)
            {
                monoState.OnStateFinished = () =>
                {
                    fsm.Trigger(transitionKey);
                };
            }
        }

        if (actions.Last().linkedState is MonoBaseState lastState)
        {
            lastState.OnStateFinished = () =>
            {
                Debug.Log("Plan GOAP finalizado.");
            };
        }

        return fsm;
    }

    private void OnPathCompleted(IEnumerable<GOAPState> sequence)
    {
        foreach (var act in sequence.Skip(1))
        {
            Debug.Log(act);
        }

        Debug.Log("WATCHDOG " + _watchdog);

        var plan = sequence.Skip(1).Select(x => x.generatingAction);

        OnPlanCompleted?.Invoke(plan);
    }

    private void OnCantCalculate()
    {
        OnCantPlan?.Invoke();
    }
    private static bool Satisfies(GOAPState current, GOAPState goal)
    {
        if (goal.generatingAction == null || goal.generatingAction.preconditions == null)
        {
            Debug.Log("Satisfies: El estado objetivo no tiene precondiciones.");
            return false;
        }

        foreach (var cond in goal.generatingAction.preconditions)
        {
            bool valid = cond.Value(current.worldModel);
            Debug.Log($"Satisfies: {cond.Key} => {(valid ? "OK" : "FALLÓ")}");
            if (!valid)
                return false;
        }

        return true;
    }


    private static float GetHeuristic(GOAPState from, GOAPState goal)
    {
        return goal.generatingAction?.preconditions
                   .Count(cond => !cond.Value(from.worldModel)) ?? float.MaxValue;
    }

    private static IEnumerable<WeightedNode<GOAPState>> Explode(
        GOAPState node,
        IEnumerable<GOAPAction> actions,
        ref int watchdog)
    {
        if (watchdog-- <= 0)
        {
            Debug.LogWarning("WATCHDOG: Se alcanzó el límite de iteraciones en GOAP.");
            return Enumerable.Empty<WeightedNode<GOAPState>>();
        }

        if (node == null || node.worldModel == null)
        {
            Debug.LogWarning("Explode: GOAPState o WorldModel es null.");
            return Enumerable.Empty<WeightedNode<GOAPState>>();
        }

        var validActions = actions
            .Where(action =>
            {
                if (action == null)
                {
                    Debug.LogWarning("Acción null.");
                    return false;
                }

                if (action.preconditions == null || action.effects == null || action.cost == null)
                {
                    Debug.LogWarning($"Acción {action.name} mal configurada.");
                    return false;
                }

                bool result = action.preconditions.Values.All(cond => cond != null && cond(node.worldModel));
                Debug.Log($"Evaluando acción {action.name}: {(result ? "válida" : "inválida")}");
                return result;
            }).ToList();

        if (!validActions.Any())
        {
            Debug.LogWarning("Explode: Ninguna acción es válida desde este estado.");
        }

        return validActions.Select(action =>
        {
            var newModel = node.worldModel.Clone();
            foreach (var effect in action.effects.Values)
            {
                effect?.Invoke(newModel);
            }

            float actionCost = action.cost(newModel);
            return new WeightedNode<GOAPState>(
                new GOAPState(newModel, action) { step = node.step + 1 },
                actionCost
            );
        }).Where(node => node != null);
    }


}
