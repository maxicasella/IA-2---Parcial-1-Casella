using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using FSM;

public class EnemyPickKnife : MonoBaseState
{
    [Header("Nodes")]
    [SerializeField] Node start;
    [SerializeField] Node end;
    Node _knifeNode;
    [SerializeField] SpatialGrid _spatialGrid;

    [Header("Components")]
    [SerializeField] Enemy _myEnemy;
    [SerializeField] SquareQuery _myQuery = null;
    public Transform knifePosition;
    [SerializeField] GameObject _knifeGO;
    [SerializeField] Animator _myAnim;
    [SerializeField] ParticleSystem _particles;
    public List<Node> path;

    [Header("Values")]
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _movementSpeed;

    AStar<Node> _astar;
    bool _calculatePath = false;
    bool _isGoalNode = false;
    bool _isRecolecting = false;
    bool _pickKnife = false;
    
    public WeaponType WeaponCollect { get {
            if (_pickKnife) return WeaponType.Knife;
            else return WeaponType.Bow;
            } }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _isRecolecting = true;
        _astar = new AStar<Node>();
        _knifeNode = FindArrowsNode();

        if (_knifeNode != null) end = _knifeNode;

        _astar.OnPathCompleted += PathCompleted;
        _astar.OnCantCalculate += PathCantCompleted;
        base.Enter(from, transitionParameters);
    }
    public override void UpdateLoop()
    {
        GoToWeapon();
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        FinishCollect();
        return base.Exit(to);
    }
    public override IState ProcessInput()
    {
        if (_pickKnife && Transitions.ContainsKey("OnEnemyPatroll"))
        {
            return Transitions["OnEnemyPatroll"];
        }

        return this;
    }
    Node FindArrowsNode()
    {
        Vector3 arrowPos = knifePosition.position;

        return FindObjectsOfType<Node>()
            .Where(n => n.neighbours != null && n.neighbours.Length > 0)
            .OrderBy(n => Vector3.Distance(n.transform.position, arrowPos))
            .FirstOrDefault();
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
    void PathCantCompleted()
    {
        _myAnim.SetBool("Walk", false);
        Debug.Log("Path calculation failed.");
    }
    public void GoToWeapon()
    {
        if (_isRecolecting && !_calculatePath)
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
                //    PickArrows();
                //    yield break;
                //}
                yield return null;
            }
            transform.position = targetPosition;
            if (node == end)
            {
                PickKnife();
                _isGoalNode = true;
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        _myAnim.SetBool("Walk", false);
        _calculatePath = false;
    }
    public void FinishCollect()
    {
        StopAllCoroutines();
        _calculatePath = false;
        _isGoalNode = false;
        _isRecolecting = false;
    }
    void PickKnife()
    {
        if (_knifeGO != null && !_pickKnife) StartCoroutine(ExecuteCollect());
    }

    IEnumerator ExecuteCollect()
    {
        _myAnim.SetBool("Walk", false);
        _particles.Play();
        Destroy(_knifeGO);
        yield return new WaitForSeconds(1f);
        _pickKnife = true;
    }
}
