using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : IState
{
    FSM _fsm;
    CharacterController _character;
    Animator _myAnim;
    public PlayerAttack(FSM fsm, CharacterController character, Animator myAnim)
    {
        _fsm = fsm;
        _character = character;
        _myAnim = myAnim;
    }
    void IState.OnEnter()
    {
        _character.attack = true;
        _myAnim.SetBool("Attack", true);
    }
    void IState.OnUpdate()
    {
        if (!_myAnim.GetBool("Attack")) _fsm.UpdateStates(States.Idle);
    }
    void IState.OnExit()
    {
        _character.attack = false;
        _myAnim.SetBool("Attack", false);
    }
}
