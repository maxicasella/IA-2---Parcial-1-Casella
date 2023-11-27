using System.Collections;
using System.Collections.Generic;
using FSM;
using System;
using System.Linq;
using UnityEngine;

public enum EnemyStates
{
    EnemyIdle,
    EnemyPatrol,
    EnemyRangeAttack,
    EnemyDeath
}

public class EnemyController : MonoBehaviour //IA2-P3
{
    [Header("States")]
    [SerializeField] EnemyPatrol _patrol;
    [SerializeField] EnemyRangeAttack _rangeAttack;
    [SerializeField] EnemyDeath _death;
    [SerializeField] EnemyIdle _idle;

    FiniteStateMachine _fsm;

    void Start()
    {
        _fsm = ConfigureFSM();
        _fsm.Active = true;
    }

    public FiniteStateMachine ConfigureFSM()
    {
        Dictionary<IState, IState[]> transitions = new Dictionary<IState, IState[]>();
        transitions.Add(_idle, new IState[] { _death, _rangeAttack, _patrol, });
        transitions.Add(_patrol, new IState[] { _death, _idle, _rangeAttack });
        transitions.Add(_rangeAttack, new IState[] { _death, _idle });
        transitions.Add(_death, new IState[] {});

        IState prevState = _idle;
        var fsm = new FiniteStateMachine(prevState, StartCoroutine);

        //IA2-P1
        var allTransitions = transitions.Aggregate(new List<Tuple<string, IState, IState>>(), (acum, current) =>
        {
            foreach (var state in current.Value)
            {
                acum.Add(Tuple.Create("On" + current.Key.Name, current.Key, state));
            }
            return acum;
        });

        foreach (var transition in allTransitions)
        {
            fsm.AddTransition(transition.Item1, transition.Item3, transition.Item2);
        }

        return fsm;
    }
}
