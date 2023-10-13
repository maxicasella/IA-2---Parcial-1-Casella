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
public class Entity : MonoBehaviour
{
    float _currentLife;
    float _currentStamina;

    [SerializeField] float _maxLife;
    [SerializeField] float _damage;
    [SerializeField] float _maxStamina;

    //Getters
    public float Life { get { return _currentLife; } }
    public float Stamina { get { return _currentStamina;} }
    public float Damage { get { return _damage; } }

    void Awake()
    {
        _currentLife = _maxLife;
        _currentStamina = _maxStamina;
    }
}
