﻿using System;
using FSM;
using UnityEngine;

public class MeleeAttackState : MonoBaseState {

    public float attackRate = .5f;

    private float _lastAttackTime; 
    
    public override void UpdateLoop() {
        if (Time.time >= _lastAttackTime + attackRate) {
            _lastAttackTime = Time.time;
            Debug.Log("Ataco");
        }
    }

    public override IState ProcessInput() {
        return this;
    }
}