using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Node : MonoBehaviour, IGridEntity
{
    [SerializeField] SpatialGrid _spatialGrid;
    [SerializeField] SquareQuery _myQuery;
    public int heuristic;
    public Node[] neighbours;

    public event Action<IGridEntity> OnMove;
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    void Start()
    {
        _spatialGrid.Add(this);
        _spatialGrid.UpdateEntity(this);
        AddNodes();
    }

    void Update()
    {
        //if(neighbours.Length == 0)
        //{
        //    _spatialGrid.UpdateEntity(this);
        //    AddNodes();
        //}
    }

    void AddNodes()
    {
        var nodes = _myQuery.Query().Select(x => x as Node).Where(x => x != this).ToList(); //IA2-P2
        neighbours = nodes.ToArray();
    }
}
