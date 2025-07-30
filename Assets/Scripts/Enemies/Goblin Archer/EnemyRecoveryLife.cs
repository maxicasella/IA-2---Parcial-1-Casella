using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using FSM;

public class EnemyRecoveryLife : MonoBaseState
{
    [Header("Nodes")]
    [SerializeField] Node start;
    [SerializeField] Node end;
    Node _safeEndNode;
    [SerializeField] SpatialGrid _spatialGrid;

    [Header("Components")]
    [SerializeField] SquareQuery _myQuery;
    [SerializeField] Animator _myAnim;
    [SerializeField] ParticleSystem _recoveryParticles;
    public Enemy _myEnemy;
    public List<Node> path;

    [Header("Values")]
    [SerializeField] float _movementSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _minRecoveryDistance;
    [SerializeField] int _recoveryValue;

    AStar<Node> _astar;
    bool _calculatePath = false;
    bool _isGoalNode = false;
    bool _isRecovery = false;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _isRecovery = true;
        _astar = new AStar<Node>();
        _safeEndNode = FindSafeNode();

        if (_safeEndNode != null) end = _safeEndNode;

        _astar.OnPathCompleted += PathCompleted;
        _astar.OnCantCalculate += PathCantCompleted;
        base.Enter(from, transitionParameters);
    }
    public override void UpdateLoop()
    {
        GoToSafePosition();
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        FinishRecovery();
        if (_astar != null)
        {
            _astar.OnPathCompleted -= PathCompleted;
            _astar.OnCantCalculate -= PathCantCompleted;
        }
        return base.Exit(to);
    }
    public override IState ProcessInput()
    {
        if (_isGoalNode && Transitions.ContainsKey("OnEnemyIdle"))
        {
            return Transitions["OnEnemyIdle"];
        }
        return this;
    }
    Node FindSafeNode()
    {
        var allNodes = FindObjectsOfType<Node>();
        Vector3 pos = transform.position;

        var nodes = allNodes
            .Where(n => n.neighbours != null && n.neighbours.Length > 0)
            .Where(n => Vector3.Distance(pos, n.transform.position) >= _minRecoveryDistance)
            .OrderByDescending(n => Vector3.Distance(pos, n.transform.position)) 
            .ToList();

        if (allNodes.Any()) return nodes.First(); 

        Debug.Log("No se encontro un nodo seguro lejano.");
        return null;
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
            PathCantCompleted();
        }
    }
    public void GoToSafePosition()
    {
        if (_isRecovery && !_calculatePath)
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
    void PathCantCompleted()
    {
        _myAnim.SetBool("Walk", false);
        Debug.Log("Path calculation failed.");
    }
    public void FinishRecovery()
    {
        StopAllCoroutines();
        _calculatePath = false;
        _isGoalNode = false;
        _isRecovery = false;
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

                //if (node == end && Vector3.Distance(transform.position, targetPosition) <= 0.1f)
                //{
                //    StartCoroutine(ExecuteRecovery());
                //    yield break;
                //}
                yield return null;
            }
            transform.position = targetPosition;
            if (node == end)
            {
                StartCoroutine(ExecuteRecovery());
                _isGoalNode = true;
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        _myAnim.SetBool("Walk", false);
        _calculatePath = false;
    }
    IEnumerator ExecuteRecovery()
    {
        _myAnim.SetBool("Walk", false);
        _recoveryParticles.Play();
        _myEnemy.RecoveryLife(_recoveryValue);

        yield return new WaitForSeconds(1f); 

        _isGoalNode = true;
    }

    void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, _minRecoveryDistance);
    }
}
