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
    Tools _activeSw;

    [SerializeField] float _maxLife;
    [SerializeField] float _maxStamina;
    [SerializeField] Character_HUD _myHud;
    [SerializeField] Character_Equipment _myInventory;


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
        _activeSw = _myInventory.GetEquippedWeapon(Tools.ToolType.Sword);
        if(_activeSw != null) _myHud.UpdateUI(_currentLife, _currentStamina, _activeSw);
        _myHud.UpdateUI(_currentLife, _currentStamina, _activeSw);
    }
    public void TakeDamage(float value)
    {
        _currentLife -= value;
    }
}
