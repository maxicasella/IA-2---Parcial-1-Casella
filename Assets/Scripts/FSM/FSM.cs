using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    IState _actualState;
    Dictionary<States, IState> _allStates = new Dictionary<States, IState>();

    public void Update()
    {
        _actualState.OnUpdate();
    }

    public void UpdateStates(States state)
    {
        if (!_allStates.ContainsKey(state)) return;

        if (_actualState != null) _actualState.OnExit();

        _actualState = _allStates[state];
        _actualState.OnEnter();
    }
    public void AddState(States state, IState action)
    {
        if (!_allStates.ContainsKey(state)) _allStates.Add(state, action);
        else _allStates[state] = action;
    }
    public States CurrentState
    {
        get
        {
            foreach (var key in _allStates) 
                if (key.Value == _actualState) return key.Key;
            
            return States.Exit;
        }
    }
}
