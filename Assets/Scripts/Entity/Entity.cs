using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    Idle,
    Movement,
    Attack,
    Damage,
    Exit
}
public class Entity : MonoBehaviour, IDamageable
{
    float _currentLife;
    float _currentStamina;

    [SerializeField] float _maxLife;
    [SerializeField] float _maxStamina;

    //Getters
    public float Life { get { return _currentLife; } }
    public float Stamina { get { return _currentStamina;} }
    void Awake()
    {
        _currentLife = _maxLife;
        _currentStamina = _maxStamina;
    }
    public void TakeDamage(float value)
    {
        _currentLife -= value;
    }
}
