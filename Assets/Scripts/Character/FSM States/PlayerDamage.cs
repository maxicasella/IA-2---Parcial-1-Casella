using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : IState
{
    FSM _fsm;
    CharacterController _character;
    Animator _myAnim;

    public PlayerDamage(FSM fsm, CharacterController character, Animator myAnim)
    {
        _fsm = fsm;
        _character = character;
        _myAnim = myAnim;
    }
    void IState.OnEnter()
    {
        throw new System.NotImplementedException();
    }
    void IState.OnUpdate()
    {
        throw new System.NotImplementedException();
    }
    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

}
