using System.Collections.Generic;
using FSM;
using UnityEngine;

public class GoapMiniTest : MonoBehaviour {

    public EnemyIdle      idleState;
    public EnemyPatrol       patrolState;
    public EnemyDeath deathState;
    public EnemyRangeAttack rangeAttack;

    private FiniteStateMachine _fsm;
    
    
    void Start() {
        OnlyPlan();
        //PlanAndExecute();
    }

    private void OnlyPlan() 
    {

    }

    //private void PlanAndExecute() {
    //    var actions = new List<GOAPAction>{
    //                                          new GOAPAction("Patrol")
    //                                             .Effect("isPlayerInSight", true)
    //                                             .LinkedState(patrolState),

    //                                          new GOAPAction("Idle")
    //                                             .Pre("isPlayerInSight", true)
    //                                             .Effect("isPlayerNear",    true)
    //                                             .LinkedState(idleState),

    //                                          new GOAPAction("Death")
    //                                             .Pre("isPlayerNear",   true)
    //                                             .Effect("isPlayerAlive", false)
    //                                             .LinkedState(deathState),

    //                                          new GOAPAction("Range Attack")
    //                                             .Pre("isPlayerNear",   true)
    //                                             .Effect("isPlayerAlive", false)
    //                                             .LinkedState(rangeAttack)
    //                                      };
        
    //    var from = new GOAPState();
    //    from.values["isPlayerInSight"] = false;
    //    from.values["isPlayerNear"]    = false;
    //    from.values["isPlayerAlive"]   = true;

    //    var to = new GOAPState();
    //    to.values["isPlayerAlive"] = false;

    //    var planner = new GoapPlanner();
    //    planner.OnPlanCompleted += OnPlanCompleted;
    //    planner.OnCantPlan      += OnCantPlan;

    //    planner.Run(from, to, actions, StartCoroutine);
    //}


    private void OnPlanCompleted(IEnumerable<GOAPAction> plan) {
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    private void OnCantPlan() {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas
    }

}
