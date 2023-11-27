using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState_Character
{
    public void OnEnter();
    public void OnUpdate();
    public void OnExit();
}
