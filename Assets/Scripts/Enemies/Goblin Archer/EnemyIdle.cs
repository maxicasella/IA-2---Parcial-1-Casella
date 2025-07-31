using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using FSM;

public class EnemyIdle : MonoBaseState //IA2-P3
{
    [Header("Values")]
    [SerializeField] float _timeToExitIdle;
    
    [Header("Components")]
    [SerializeField] Enemy _myEnemy;
    [SerializeField] SquareQuery _myQuery;
    [SerializeField] Animator _myAnim;

    float _counter;
    bool _toAttack;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _counter = 0;
        base.Enter(from, transitionParameters);
    }
     void Update()
    {
        _counter = Time.time;
    }
    public override void UpdateLoop()
    {
        DetectPlayer();
        if (_counter >= _timeToExitIdle) FinishState();
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        _myAnim.SetBool("Walk", false);
        _counter = 0;
        return base.Exit(to);
    }
    public override IState ProcessInput() 
    {
        //if (_myEnemy.CurrentLife <= 0 && Transitions.ContainsKey("OnEnemyDeath"))
        //{
        //    return Transitions["OnEnemyDeath"];
        //}

        //if (_toAttack && Transitions.ContainsKey("OnEnemyRangeAttack"))
        //{
        //    return Transitions["OnEnemyRangeAttack"];
        //}
        //else if(_counter >= _timeToExitIdle && Transitions.ContainsKey("OnEnemyPatrol"))
        //{
        //        return Transitions["OnEnemyPatrol"];
        //}

        return this;
        //throw new NotImplementedException();
    }
    bool DetectPlayer() //IA2-P2
    {
        var target = _myQuery.Query().Select(x => x as CharacterController).Where(x => x != null).FirstOrDefault();

        if (target != null) return _toAttack = true;
        else return _toAttack = false;
    }


}
