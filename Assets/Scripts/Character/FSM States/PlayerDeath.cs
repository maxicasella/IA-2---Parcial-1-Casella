using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : IState_Character
{
    FSM_Character _fsm;
    CharacterController _character;
    Animator _myAnim;

    public PlayerDeath(FSM_Character fsm, CharacterController character, Animator myAnim)
    {
        _fsm = fsm;
        _character = character;
        _myAnim = myAnim;
    }
    void IState_Character.OnEnter()
    {
        throw new System.NotImplementedException();
    }
    void IState_Character.OnUpdate()
    {
        throw new System.NotImplementedException();
    }
    void IState_Character.OnExit()
    {
        throw new System.NotImplementedException();
    }
}
