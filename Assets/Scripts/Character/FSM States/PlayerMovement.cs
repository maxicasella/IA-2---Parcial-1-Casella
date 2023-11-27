using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : IState_Character
{
    FSM_Character _fsm;
    CharacterController _character;
    Animator _myAnim;
    bool _isMoving;

    public PlayerMovement(FSM_Character fsm, CharacterController character, Animator myAnim)
    {
        _fsm = fsm;
        _character = character;
        _myAnim = myAnim;
    }
    void IState_Character.OnEnter()
    {
        _character.move = true;
        Move(_character.xInput, _character.yInput);
        _myAnim.SetBool("Run", true);
    }
    void IState_Character.OnUpdate()
    {
        if (_isMoving) Move(_character.xInput, _character.yInput);
        else _fsm.UpdateStates(States.Idle);
    }
    void IState_Character.OnExit()
    {
        _character.move = false;
        _myAnim.SetBool("Run", false);
    }
    void Move(float verticalInput, float horizontalInput)
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement.Normalize();

        _character.myRb.velocity = movement * _character.MovementSpeed;

        if (movement.magnitude > 0)
        {
            _isMoving = true;
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            _character.transform.rotation = Quaternion.RotateTowards(_character.transform.rotation, toRotation, 360f);
        }
        else _isMoving = false;
    }
}
