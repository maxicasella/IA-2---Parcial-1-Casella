using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class GOAPAction {

    public Dictionary<string, Func<WorldModel, bool>> preconditions { get; private set; }
    public Dictionary<string, Action<WorldModel>> effects       { get; private set; }
    public string                   name          { get; private set; }
    public Func<WorldModel, float> cost = _ => 1f;
    public IState                   linkedState   { get; private set; }


    public GOAPAction(string name) {
        this.name     = name;
        cost = _ => 1f;
        preconditions = new Dictionary<string, Func<WorldModel, bool>>();
        effects       = new Dictionary<string, Action<WorldModel>>();
    }

    public GOAPAction Cost(Func<WorldModel, float> costFunc)
    {
        this.cost = costFunc;
        return this;
    }

    public GOAPAction Pre(string key, Func<WorldModel, bool> condition) {
        preconditions[key] = condition;
        return this;
    }

    public GOAPAction Effect(string key, Action<WorldModel> effect) {
        effects[key] = effect;
        return this;
    }

    public GOAPAction LinkedState(IState state) {
        linkedState = state;
        return this;
    }
}
