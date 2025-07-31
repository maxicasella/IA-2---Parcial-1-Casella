using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using FSM;

public class EnemyMeleeKnifeAttack : MonoBaseState
{
    [Header("Components")]
    [SerializeField] Enemy _myEnemy;
    [SerializeField] SquareQuery _myQuery = null;
    [SerializeField] Animator _myAnim;

    [Header("Values")]
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _cooldown;
    [SerializeField] float _attackRange = 1.5f;

    bool _isAttack = false;
    float _nextAttackTime;
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _myAnim.SetBool("Knife Attack", true);
        base.Enter(from, transitionParameters);
    }
    public override void UpdateLoop()
    {
        AttackPlayer();
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        _myAnim.SetBool("Knife Attack", false);
        return base.Exit(to);
    }
    public override IState ProcessInput()
    {
        //if (_myEnemy.CurrentLife <= 0 && Transitions.ContainsKey("OnEnemyDeath"))
        //{
        //    return Transitions["OnEnemyDeath"];
        //}
        //if (!_isAttack && Transitions.ContainsKey("OnEnemyPatrol"))
        //{
        //    return Transitions["OnEnemyPatrol"];
        //}
        return this;
        //throw new NotImplementedException();
    }
    void AttackPlayer()
    {
        var target = _myQuery.Query()
              .Select(x => x as CharacterController)
              .Where(x => x != null)
              .FirstOrDefault();

        if (target == null)
        {
            _isAttack = false;
            _myAnim.SetBool("Knife Attack", false);
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.Position);

        if (distanceToTarget > _attackRange)
        {
            _isAttack = false;
            _myAnim.SetBool("Knife Attack", false);
            FinishState();
            return;
        }

        Vector3 targetDir = target.Position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _myQuery.targetGrid.UpdateEntity(_myEnemy);

        if (Time.time >= _nextAttackTime)
        {
            _isAttack = true;
            _myAnim.SetBool("Knife Attack", true);
            _nextAttackTime = Time.time + _cooldown;
        }
    }
}
