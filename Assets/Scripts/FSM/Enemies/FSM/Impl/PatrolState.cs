using System;
using FSM;

public class PatrolState : MonoBaseState {

    private Entity _player;
    
    
    private void Awake() {
        _player = FindObjectOfType<Entity>();
    }
    
    public override void UpdateLoop() {
        //TODO: patrullo
    }

    public override IState ProcessInput() {
        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;

        if (sqrDistance < 100f && Transitions.ContainsKey("OnChaseState")) {
            return Transitions["OnChaseState"];
        }

        return this;
    }
}
