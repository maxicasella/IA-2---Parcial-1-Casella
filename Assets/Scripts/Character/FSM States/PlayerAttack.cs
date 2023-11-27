using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : IState_Character
{
    FSM_Character _fsm;
    CharacterController _character;
    Animator _myAnim;
    public PlayerAttack(FSM_Character fsm, CharacterController character, Animator myAnim)
    {
        _fsm = fsm;
        _character = character;
        _myAnim = myAnim;
    }
    void IState_Character.OnEnter()
    {
        _character.attack = true;
        _myAnim.SetBool("Attack", true);
    }
    void IState_Character.OnUpdate()
    {
        if (!_myAnim.GetBool("Attack")) _fsm.UpdateStates(States.Idle);
    }
    void IState_Character.OnExit()
    {
        _character.attack = false;
        _myAnim.SetBool("Attack", false);
    }
}
