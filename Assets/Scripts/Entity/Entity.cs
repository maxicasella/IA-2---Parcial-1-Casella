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
    public BaseWeapon _activeWeapon;

    [SerializeField] float _maxLife;
    [SerializeField] float _maxStamina;
    [SerializeField] Character_HUD _myHud;

    //Getters
    public float Life { get { return _currentLife; } }
    public float Stamina { get { return _currentStamina;} }
    void Awake()
    {
        _currentLife = _maxLife;
        _currentStamina = _maxStamina;
    }

    void Update()
    {
        _myHud.UpdateUI(_currentLife, _currentStamina, _activeWeapon);
    }
    public void TakeDamage(float value)
    {
        _currentLife -= value;
    }
}
