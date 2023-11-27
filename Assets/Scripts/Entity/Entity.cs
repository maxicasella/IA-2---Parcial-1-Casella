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
    [SerializeField] GameObject _dmgParticles;
    [SerializeField] Transform _particlesPoint;

    public int GetNormalDamage { get { return _activeSw.NormalDamage; } }
    public int GetCriticalDamage { get { return _activeSw.CriticalDamage; } }

    public Tools ActiveSword { get { return _activeSw; } }

    //Getters
    public float Life { get { return _currentLife; } }

    public float MaxLife { get { return _maxLife; } }
    public float Stamina { get { return _currentStamina; } }

    void Awake()
    {
        _currentLife = _maxLife;
        _currentStamina = _maxStamina;
    }

    void Update()
    {
        _activeSw = _myInventory.GetEquippedWeapon(Tools.ToolType.Sword);
        if (_activeSw != null) _myHud.UpdateUI(_currentLife, _currentStamina, _activeSw);
        _myHud.UpdateUI((_currentLife/_maxLife), (_currentStamina/_maxStamina), _activeSw);
    }
    public void TakeDamage(float value)
    {
        _currentLife -= value;
        Instantiate(_dmgParticles, _particlesPoint.transform);
        GameManager.Instance.GameOver();
    }

    public void TakeDamage(float value, GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public void WeaponDurability(float value)
    {
        _activeSw.Durability = value;
    }

    public void Repair()
    {
        if(_activeSw._actualDurability < _activeSw._maxDurability) _activeSw._actualDurability = _activeSw._maxDurability;
    }
}
