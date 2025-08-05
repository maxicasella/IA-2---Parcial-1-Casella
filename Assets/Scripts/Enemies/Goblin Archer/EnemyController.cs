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
    [SerializeField] WeaponType _weapon;
    string weapon;

    [Header("Damages")]
    [SerializeField] int _kickDamage;
    [SerializeField] int _knifeDamage;

    [Header("Nodes")]
    [SerializeField] Node start;
    [SerializeField] Node end;
    [SerializeField] SpatialGrid _spatialGrid;

    [Header("Components")]
    [SerializeField] Enemy _myEnemy;
    [SerializeField] SquareQuery _myQuery;
    [SerializeField] Animator _myAnim;
    public List<Node> path;

    [Header("Movement")]
    [SerializeField] float _movementSpeed;
    [SerializeField] float _rotationSpeed;
    [Header("Distances to Objects")]
    float _distanceToPlayer;
    float _distanceToArrows;
    float _distanceToKnife;

    [Header("GOAP")]
    [SerializeField] float _replanTime;
    Coroutine _replanRoutine;
    WorldModel _lastWorldModel;

    void Start()
    {
        weapon = _weapon.ToString();
        //_fsm = ConfigureFSM();
        //_fsm.Active = true;
    }

    void Update()
    {
        if (_myEnemy.CurrentLife <= 0 && !(_fsm.CurrentState is EnemyDeath))
        {
            _fsm.Active = false;
            _fsm = new FiniteStateMachine(_death, StartCoroutine);
            _fsm.Active = true;
            return;
        }
        if (_fsm == null || !_fsm.Active)
            GOAPPlan();
    }

    //public FiniteStateMachine ConfigureFSM()
    //{
    //    Dictionary<IState, IState[]> transitions = new Dictionary<IState, IState[]>();
    //    transitions.Add(_idle, new IState[] { _death, _rangeAttack, _patrol, });
    //    transitions.Add(_patrol, new IState[] { _death, _idle, _rangeAttack });
    //    transitions.Add(_rangeAttack, new IState[] { _death, _idle, _patrol });
    //    transitions.Add(_death, new IState[] { });

    //    IState prevState = _idle;
    //    var fsm = new FiniteStateMachine(prevState, StartCoroutine);

    //    //IA2-P1
    //    var allTransitions = transitions.Aggregate(new List<Tuple<string, IState, IState>>(), (acum, current) =>
    //    {
    //        foreach (var state in current.Value)
    //        {
    //            acum.Add(Tuple.Create("On" + state.Name, current.Key, state));
    //        }
    //        return acum;
    //    });

    //    foreach (var transition in allTransitions)
    //    {
    //        fsm.AddTransition(transition.Item1, transition.Item2, transition.Item3);
    //    }

    //    return fsm;
    //}
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

        var currentWorldModel = new WorldModel
        {
            life = _myEnemy.CurrentLife,
            maxLife = _myEnemy.MaxLife,
            alive = _target.Alive,
            weapon = this.weapon,
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
        Debug.Log("Weapon actual: " + currentWorldModel.weapon);
        if (_lastWorldModel != null && _lastWorldModel.Equals(currentWorldModel))
        {
            Debug.Log("World Model sin cambios.");
            return;
        }
        var actions = new List<GOAPAction>
        {
            //Acciones con efectos, precondiciones, costos y estados linkeados    
            new GOAPAction("Patrol")
                .Effect("isPlayerInRange", wm =>
                { wm.distanceToPlayer = Mathf.Min(wm.distanceToPlayer, wm.rangeAttackDistance - 0.1f);
                })
                .Cost(_ => 1f)
                .LinkedState(_patrol),

            new GOAPAction("Range Attack")
                .Pre("isPlayerInRange", wm => wm.distanceToPlayer <= wm.rangeAttackDistance)
                .Pre("hasArrows", wm => wm.arrows > 0)
                .Effect("isPlayerAlive", wm => wm.alive = false)
                .Cost(wm => 1f + (1f / Mathf.Max(1, wm.rangeAttackDistance)))
                .LinkedState(_rangeAttack),

            new GOAPAction("Knife Melee Attack")
                .Pre("isPlayerNear", wm => wm.distanceToPlayer <= wm.meleeAttackDistance)
                .Pre("hasKnife", wm => wm.weapon == "Knife")
                .Pre("isHealthy", wm => wm.life > wm.maxLife * 0.25f)
                .Effect("isPlayerAlive", wm => wm.alive = false)
                .Cost(wm => 1f + (1f / Mathf.Max(1f, wm.meleeKickDamage)))
                .LinkedState(_meleeKnifeAttack),

            new GOAPAction("Kick Melee Attack")
                .Pre("isPlayerNear", wm => wm.distanceToPlayer <= wm.meleeAttackDistance)
                .Pre("hasKnife", wm => wm.weapon != "Knife")
                .Pre("isHealthy", wm => wm.life > wm.maxLife * 0.25f)
                .Effect("isPlayerAlive", wm => wm.alive = false)
                .Cost(wm => 1f + (1f / Mathf.Max(1f, wm.meleeKickDamage)))
                .LinkedState(_meleeKickAttack),

            new GOAPAction("Pick Arrows")
                .Pre("noArrows", wm => wm.arrows <= 0)
                .Effect("hasArrows", wm => wm.arrows = wm.maxArrows)
                .Cost(wm => 1f + wm.distanceToArrows)
                .LinkedState(_pickArrows),

            new GOAPAction("Recovery Life")
                .Pre("lowHP", wm => wm.life <= (0.25f * wm.maxLife))
                .Effect("recoveryHP", wm => wm.life = Mathf.Min(wm.maxLife, wm.life + (0.5f * wm.maxLife)))
                .Cost(_ => 1f)
                .LinkedState(_recoveryLife),
        };

        GOAPState initialState = new GOAPState(currentWorldModel.Clone());

        var desiredWorldModel = currentWorldModel.Clone();

        var killAction = new GOAPAction("KillPlayerGoal")
            .Pre("isPlayerAlive", wm => wm.alive == false);

        var desiredState = new GOAPState(desiredWorldModel, killAction);

        var planner = new GoapPlanner();
        planner.OnPlanCompleted += OnPlanCompleted;
        planner.OnCantPlan += OnCantPlan;
        planner.Run(initialState, desiredState, actions, StartCoroutine);
    }

    void GOAPExecutePlan(IEnumerable<GOAPAction> plan)
    {
        if (!_target.Alive) return;
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    private void OnPlanCompleted(IEnumerable<GOAPAction> plan)
    {
        //_fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        //_fsm.Active = true;
        Debug.Log("Plan GOAP completado.");
        Debug.Log("Nuevo plan GOAP:");
        foreach (var action in plan)
            Debug.Log(" - " + action.name);
        GOAPExecutePlan(plan);
        if (_replanRoutine == null)
            _replanRoutine = StartCoroutine(PeriodicReplan());
    }
    private void OnCantPlan()
    {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas
        Debug.LogWarning("No se pudo generar un plan con GOAP.");
    }

    IEnumerator PeriodicReplan()
    {
        while (true)
        {
            yield return new WaitForSeconds(_replanTime);

            if (_fsm == null || _fsm.Active)
            {
                Debug.Log("Replanteo");
                GOAPPlan();
            }
        }
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