using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IState
{
    FSM _fsm;
    CharacterController _character;
    Animator _myAnim;

    public PlayerIdle(FSM fsm, CharacterController character, Animator myAnim)
    {
        _fsm = fsm;
        _character = character;
        _myAnim = myAnim;
    }
    void IState.OnEnter()
    {
        _character.idle = true;
        _myAnim.SetBool("Idle", true);
    }

    void IState.OnUpdate()
    {
        //Empty
    }
    void IState.OnExit()
    {
        _character.idle = false;
        _myAnim.SetBool("Idle", false);
    }
}
