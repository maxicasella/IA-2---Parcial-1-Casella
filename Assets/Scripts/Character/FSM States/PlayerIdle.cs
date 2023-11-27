using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IState_Character
{
    FSM_Character _fsm;
    CharacterController _character;
    Animator _myAnim;

    public PlayerIdle(FSM_Character fsm, CharacterController character, Animator myAnim)
    {
        _fsm = fsm;
        _character = character;
        _myAnim = myAnim;
    }
    void IState_Character.OnEnter()
    {
        _character.idle = true;
        _myAnim.SetBool("Idle", true);
    }

    void IState_Character.OnUpdate()
    {
        //Empty
    }
    void IState_Character.OnExit()
    {
        _character.idle = false;
        _myAnim.SetBool("Idle", false);
    }
}
