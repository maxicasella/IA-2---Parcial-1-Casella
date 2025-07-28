using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using FSM;

public class EnemyRangeAttack : MonoBaseState //IA2-P3
{
    [Header("Components")]
    [SerializeField] Enemy _myEnemy;
    [SerializeField] SquareQuery _myQuery = null;
    [SerializeField] GameObject _arrowPrefab;
    [SerializeField] Transform _shootPosition;
    [SerializeField] Animator _myAnim;

    [Header("Values")]
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _shootCooldown;
    public int maxArrows;
    int _actualArrows;

    public int ActualArrows { get { return _actualArrows; } }
    bool _isAttack = false;
    float _nextShootTime;
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _actualArrows = maxArrows;
        _myAnim.SetBool("Range Attack", true);

        base.Enter(from, transitionParameters);
    }
    public override void UpdateLoop()
    {
        DetectPlayer();
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        _myAnim.SetBool("Range Attack", false);
        return base.Exit(to);
    }
    public override IState ProcessInput()
    {
        if (_myEnemy.CurrentLife <= 0 && Transitions.ContainsKey("OnEnemyDeath"))
        {
            return Transitions["OnEnemyDeath"];
        }

       if(!_isAttack && Transitions.ContainsKey("OnEnemyPatrol"))
       {
            return Transitions["OnEnemyPatrol"];
       }

        return this;
    }

    void DetectPlayer() //IA2-P2
    {
        var target = _myQuery.Query().Select(x => x as CharacterController).Where(x => x != null).FirstOrDefault();

        if (target == null)
        {
            _isAttack = false;
            _myAnim.SetBool("Range Attack", false);
            return;
        }
        else
        {
            Vector3 targetDir = target.Position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _myQuery.targetGrid.UpdateEntity(_myEnemy);

            if (Time.time >= _nextShootTime && _actualArrows > 0)
            {
                _isAttack = true;
                _myAnim.SetBool("Range Attack", true);
                Shoot();
                _actualArrows--;
                _nextShootTime = Time.time + _shootCooldown;
            }
        }
    }
    void Shoot()
    {
        Instantiate(_arrowPrefab, _shootPosition.position,_shootPosition.rotation);
    }
    public void PickArrows()
    {
        _actualArrows = maxArrows;
    }
}