using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using FSM;

public class EnemyPatrol : MonoBaseState //IA2-P3
{
    [Header("Nodes")]
    [SerializeField] Node start;
    [SerializeField] Node end;
    [SerializeField] SpatialGrid _spatialGrid;

    [Header("Components")]
    [SerializeField] SquareQuery _myQuery;
    [SerializeField] Animator _myAnim;
    public Enemy _myEnemy;
    public List<Node> path;

    [Header("Values")]
    [SerializeField] float _movementSpeed;
    [SerializeField] float _rotationSpeed;

    bool _calculatePath = false;
    bool _isPatrol;
    bool _toAttack;
    bool _isGoalNode = false;
   
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _isPatrol = true;
        _myAnim.SetBool("Walk", true);
        base.Enter(from, transitionParameters);
    }
    public override void UpdateLoop()
    {
        PatrolLogic();
        ResumePatrol();
        DetectPlayer();
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        StopPatrol();
        _myAnim.SetBool("Walk", false);
        return base.Exit(to);
    }
    public override IState ProcessInput()
    {
        if (_myEnemy.CurrentLife <= 0 && Transitions.ContainsKey("OnEnemyDeath"))
        {
            return Transitions["OnEnemyDeath"];
        }
        
        if(_isGoalNode && Transitions.ContainsKey("OnEnemyIdle"))
        {
            return Transitions["OnEnemyIdle"];
        }

        if (_toAttack && Transitions.ContainsKey("OnEnemyRangeAttack"))
        {
            return Transitions["OnEnemyRangeAttack"];
        }

        return this;
    }

    public void PatrolLogic()
    {
        if (_isPatrol && !_calculatePath)
        {
            StartCoroutine(AStar.CalculatePath(start, x => x == end, x => x.neighbours.Select(x => new WeightedNode<Node>(x, 1)),
                                x => x.heuristic, PathCompleted, PathCantCompleted));
        }
    }
    public void StopPatrol()
    {
        StopAllCoroutines();
        _calculatePath = false;
        _isGoalNode = false;
        _isPatrol = false; 
    }

    public void ResumePatrol()
    {
        _isPatrol = true; 
    }

    void PathCompleted(IEnumerable<Node> newPath)
    {
        _calculatePath = true;
        path = new List<Node>(newPath);

        if (path.Count > 0)
        {
            _myAnim.SetBool("Walk", true);
            StartCoroutine(FollowPath());
        }
        else
        {
            PathCantCompleted();
        }
    }

    void PathCantCompleted()
    {
        _myAnim.SetBool("Walk", false);
        Debug.Log("Path calculation failed.");
    }

    IEnumerator FollowPath()
    {
        foreach (var node in path)
        {
             Vector3 targetPosition = node.transform.position;

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                Vector3 moveDirection = (targetPosition - transform.position).normalized;

                if (moveDirection != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed); 
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _movementSpeed);
                _spatialGrid.UpdateEntity(_myEnemy);

                if (node == end && Vector3.Distance(transform.position, targetPosition) <= 0.1f)
                {
                    _isGoalNode = true;
                    yield break; 
                }
                yield return null;
            }
            transform.position = targetPosition;
            if (node == end)
            {
                _isGoalNode = true;
                yield break; 
            }
            yield return new WaitForSeconds(0.1f);
        }
        _myAnim.SetBool("Walk", false);
        _calculatePath = false;
    }

    bool DetectPlayer() //IA2-P2
    {
        var target = _myQuery.Query().Select(x => x as CharacterController).Where(x => x != null).FirstOrDefault();

        if (target != null) return _toAttack = true;
        else return _toAttack = false;
    }
}
