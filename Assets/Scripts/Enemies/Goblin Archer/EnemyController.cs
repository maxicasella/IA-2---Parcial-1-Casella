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
    [SerializeField] Enemy _myEnemy;
    [SerializeField] SquareQuery _myQuery;
    [Header("States")]
    [SerializeField] EnemyPatrol _patrol;
    [SerializeField] EnemyRangeAttack _rangeAttack;
    [SerializeField] EnemyDeath _death;
    [SerializeField] EnemyIdle _idle;
    [SerializeField] EnemyMeleeKickAttack _meleeKickAttack;
    [SerializeField] EnemyMeleeKnifeAttack _meleeKnifeAttack;
    [SerializeField] EnemyPickArrows _pickArrows;
    [SerializeField] EnemyPickKnife _pickKnife;
    [SerializeField] EnemyRecoveryLife _recoveryLife;

    FiniteStateMachine _fsm;

    [Header("Target")]
    [SerializeField] Entity _target;

    [Header("Attack Ranges")]
    [SerializeField] float _meleeDistance;
    [SerializeField] float _rangeDistance;

    [Header("Damages")]
    [SerializeField] float _kickDamage;
    [SerializeField] float _knifeDamage;

    [Header("Distances to Objects")]
    float _distanceToPlayer;
    float _distanceToArrows;
    float _distanceToKnife;

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
        transitions.Add(_rangeAttack, new IState[] { _death, _idle, _patrol });
        transitions.Add(_death, new IState[] { });

        IState prevState = _idle;
        var fsm = new FiniteStateMachine(prevState, StartCoroutine);

        //IA2-P1
        var allTransitions = transitions.Aggregate(new List<Tuple<string, IState, IState>>(), (acum, current) =>
        {
            foreach (var state in current.Value)
            {
                acum.Add(Tuple.Create("On" + state.Name, current.Key, state));
            }
            return acum;
        });

        foreach (var transition in allTransitions)
        {
            fsm.AddTransition(transition.Item1, transition.Item2, transition.Item3);
        }

        return fsm;
    }
    void GOAPPlan()
    {
        //Distancias player y armas
        var target = _myQuery.Query()
                             .OfType<CharacterController>()
                             .OrderBy(x => Vector3.Distance(transform.position, x.Position))
                             .FirstOrDefault();

        _distanceToPlayer = target != null
                                        ? Vector3.Distance(transform.position, target.Position)
                                        : float.MaxValue;
        _distanceToArrows = _pickArrows != null && _pickArrows.arrowsPosition != null
                                  ? Vector3.Distance(transform.position, _pickArrows.arrowsPosition.position)
                                  : float.MaxValue;
        _distanceToKnife = _pickKnife != null && _pickKnife.knifePosition != null
                                ? Vector3.Distance(transform.position, _pickKnife.knifePosition.position)
                                : float.MaxValue;

        var worldModel = new WorldModel
        {
            life = _myEnemy.CurrentLife,
            maxLife = _myEnemy.MaxLife,
            alive = _target.Alive,
            weapon = _pickKnife.WeaponCollect.ToString(),
            arrows = _rangeAttack.ActualArrows,
            maxArrows = _rangeAttack.maxArrows,
            rangeAttackDistance = _rangeDistance,
            meleeAttackDistance = _meleeDistance,
            meleeKickDamage = _kickDamage,
            meleeKnifeDamage = _knifeDamage,
            distanceToArrows = _distanceToArrows,
            distanceToKnife = _distanceToKnife,
            distanceToPlayer = _distanceToPlayer
        };

        GOAPState initialState = new GOAPState(worldModel.Clone());

        var desiredWorldModel = worldModel.Clone();
        desiredWorldModel.alive = false;

        GOAPState desiredState = new GOAPState(desiredWorldModel);

        var actions = new List<GOAPAction>
        {
            //Incluir todas las acciones con efectos, precondiciones, costos y estados linkeados    
            new GOAPAction("Patrol")
                .Effect("isPlayerInRange", wm =>
                { wm.distanceToPlayer = Mathf.Min(wm.distanceToPlayer, wm.rangeAttackDistance - 0.1f);
                })
                .Effect("isPlayerNear", wm =>
                { wm.distanceToPlayer = Mathf.Min(wm.distanceToPlayer, wm.meleeAttackDistance - 0.1f);
                })
                .Cost(_ => 1f)
                .LinkedState(_patrol),

            new GOAPAction("Range Attack")
                .Pre("isPlayerInRange", wm => wm.distanceToPlayer <= wm.rangeAttackDistance)
                .Pre("hasArrows", wm => wm.arrows > 0)
                .Effect("isPlayerAlive", wm => wm.alive = false)
                .Cost(wm => 1f + (1f / (wm.arrows > 0 ? wm.rangeAttackDistance : 1f)))
                .LinkedState(_rangeAttack),

            new GOAPAction("Knife Melee Attack")
                .Pre("isPlayerNear", wm => wm.distanceToPlayer <= wm.meleeAttackDistance)
                .Pre("hasKnife", wm => wm.weapon == "Knife")
                .Effect("isPlayerAlive", wm => wm.alive = false)
                .Cost(wm => 1f + (1f / Mathf.Max(1f, wm.meleeKnifeDamage)))
                .LinkedState(_meleeKnifeAttack),

            new GOAPAction("Kick Melee Attack")
                .Pre("isPlayerNear", wm => wm.distanceToPlayer <= wm.meleeAttackDistance)
                .Pre("noKnife", wm => wm.weapon != "Knife")
                .Effect("isPlayerAlive", wm => wm.alive = false)
                .Cost(wm => 1f + (1f / Mathf.Max(1f, wm.meleeKickDamage)))
                .LinkedState(_meleeKickAttack),

            new GOAPAction("Pick Arrows")
                .Pre("noArrows", wm => wm.arrows <= 0)
                .Effect("hasArrows", wm => wm.arrows = wm.maxArrows)
                .Cost(wm => 1f + wm.distanceToArrows)
                .LinkedState(_pickArrows),

            new GOAPAction("Pick Knife")
                .Pre("noKnife", wm => wm.weapon != "Knife")
                .Effect("hasKnife", wm => wm.weapon = "Knife")
                .Cost(wm => 1f + wm.distanceToKnife)
                .LinkedState(_pickKnife),

            new GOAPAction("Recover Life")
                .Pre("lowHP", wm => wm.life <= (0.25f * wm.maxLife))
                .Effect("recoverHP", wm => wm.life = Mathf.Min(wm.maxLife, wm.life + (0.5f * wm.maxLife)))
                .Cost(_ => 1f)
                .LinkedState(_recoveryLife)
        };

        var planner = new GoapPlanner();
        planner.OnPlanCompleted += OnPlanCompleted;
        planner.OnCantPlan += OnCantPlan;
        planner.Run(initialState, desiredState, actions, StartCoroutine);
    }

    void GOAPExecutePlan(IEnumerable<GOAPAction> plan)
    {
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    private void OnPlanCompleted(IEnumerable<GOAPAction> plan)
    {
        //_fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        //_fsm.Active = true;
        Debug.Log("Plan GOAP completado.");
        GOAPExecutePlan(plan);
    }
    private void OnCantPlan()
    {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas
        Debug.LogWarning("No se pudo generar un plan GOAP.");
    }
    void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, _meleeDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pos, _rangeDistance);
    }
}