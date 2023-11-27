using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour, IGridEntity
{
    FSM_Character _fsm;

    [SerializeField] float _movementSpeed;
    [SerializeField] Animator _myAnim;
    [SerializeField] SpatialGrid _spatialGrid;
    //States Bools
    public bool idle;
    public bool move;
    public bool attack;
    public bool damage;
    public bool death;

    public Rigidbody myRb;
    float _xInput;
    float _yInput;

    public event Action<IGridEntity> OnMove;

    
    //Getters
    public float MovementSpeed {  get { return _movementSpeed; } }
    public float xInput { get { return _xInput; } }
    public float yInput { get { return _yInput; } }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    void Awake()
    {
        _fsm = new FSM_Character();
        _fsm.AddState(States.Idle, new PlayerIdle(_fsm, this, _myAnim));
        _fsm.AddState(States.Movement, new PlayerMovement(_fsm, this, _myAnim));
        _fsm.AddState(States.Attack, new PlayerAttack(_fsm, this, _myAnim));
        _fsm.UpdateStates(States.Idle);
    }

    void Start()
    {
        _spatialGrid.Add(this);
    }

    void Update()
    {
        _xInput = Input.GetAxis("Vertical");
        _yInput = Input.GetAxis("Horizontal");

        if (xInput != 0 || yInput != 0) _fsm.UpdateStates(States.Movement);
        if (Input.GetKeyDown(KeyCode.Mouse0)) _fsm.UpdateStates(States.Attack);
        _fsm.Update();
        _spatialGrid.UpdateEntity(this);
    }
}
