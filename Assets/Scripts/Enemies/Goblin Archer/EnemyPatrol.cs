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

    AStar<Node> _astar;
    public bool _calculatePath = false;
    public bool _isPatrol;
    public bool _toAttack;
    public bool _isGoalNode = false;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        Debug.Log("Enter Patrol");
        _isPatrol = true;
        _calculatePath = false;
        _toAttack = false;
        _astar = new AStar<Node>();
        _astar.OnPathCompleted += PathCompleted;
        _astar.OnCantCalculate += PathCantCompleted;
        base.Enter(from, transitionParameters);
    }
    public override void UpdateLoop()
    {
        ResumePatrol();
        PatrolLogic();
        DetectPlayer();
        if (_toAttack)
        {
            FinishState();
            Debug.Log("Patrol: player detected -> Exit");
            return;
        }
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        StopPatrol();
        _myAnim.SetBool("Walk", false);

        if (_astar != null)
        {
            Debug.Log("Desuscribo Astar");
            _astar.OnPathCompleted -= PathCompleted;
            _astar.OnCantCalculate -= PathCantCompleted;
        }
        Debug.Log("Exit Patrol");
        return base.Exit(to);
    }
    public override IState ProcessInput()
    {
        //if (_myEnemy.CurrentLife <= 0 && Transitions.ContainsKey("OnEnemyDeath"))
        //{
        //    return Transitions["OnEnemyDeath"];
        //}
        
        //if(_isGoalNode && Transitions.ContainsKey("OnEnemyIdle"))
        //{
        //    return Transitions["OnEnemyIdle"];
        //}

        //if (_toAttack && Transitions.ContainsKey("OnEnemyRangeAttack"))
        //{
        //    return Transitions["OnEnemyRangeAttack"];
        //}

        return this;
    //    throw new NotImplementedException();
    }

    public void PatrolLogic()
    {
        if (_isPatrol && !_calculatePath)
        {
            _calculatePath = true;

            StartCoroutine(_astar.Run(
                start,
                x => x == end,
                x => x.neighbours.Select(n => new WeightedNode<Node>(n, 1)),
                x => x.heuristic
            ));
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
        path = new List<Node>(newPath);

        if (path.Count > 0)
        {
            _myAnim.SetBool("Walk", true);
            StartCoroutine(FollowPath());
        }
        else
        {
            Debug.Log("Path nodes: " + path.Count.ToString());
            PathCantCompleted();
        }
    }

    void PathCantCompleted()
    {
        Debug.LogWarning($"No se pudo calcular el path de '{start}' a '{end}'.");

        if (start == null || end == null)
            Debug.LogError("Start o End es null.");
        else if (start.neighbours == null || !start.neighbours.Any())
            Debug.LogError("Start no tiene vecinos definidos.");
        _myAnim.SetBool("Walk", false);
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
                    _myAnim.SetBool("Walk", false);
                    _isPatrol = false;
                    _calculatePath = false;
                    _isGoalNode = true;
                    yield break; 
                }
                yield return null;
            }
            transform.position = targetPosition;
            if (node == end)
            {   _myAnim.SetBool("Walk", false);
                _isPatrol = false;
                _calculatePath = false;
                _isGoalNode = true;
                yield break; 
            }
            yield return new WaitForSeconds(0.01f);
        }
        _myAnim.SetBool("Walk", false);
        _calculatePath = false;
    }

    bool DetectPlayer() //IA2-P2
    {
        var target = _myQuery.Query().Select(x => x as CharacterController).Where(x => x != null).FirstOrDefault();

        if (target != null)
        {
            _myAnim.SetBool("Walk", false);
            return _toAttack = true;
        }
        else return _toAttack = false;
    }
}
