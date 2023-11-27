using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;

public class EnemyDeath : MonoBaseState //IA2-P3
{
    [SerializeField] Animator _myAnim;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        _myAnim.SetBool("Death", true);
        base.Enter(from, transitionParameters);
    }
    public override void UpdateLoop()
    {
        throw new NotImplementedException();
    }
    public override IState ProcessInput()
    {
        return this;
    }

}
