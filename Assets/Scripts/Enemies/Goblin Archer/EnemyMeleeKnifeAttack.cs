using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using FSM;

public class EnemyMeleeKnifeAttack : MonoBaseState
{
    public override IState ProcessInput()
    {
        throw new NotImplementedException();
    }

    public override void UpdateLoop()
    {
        throw new NotImplementedException();
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
    }
    public override Dictionary<string, object> Exit(IState to)
    {
        return base.Exit(to);
    }
}
