using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] Entity _myEntity;
    float _normalDmg;
    float _critcalDmg;
    public float NormalDamage { get { return _normalDmg; } }
    public float CriticalDamage { get { return _critcalDmg; } }

    void Start()
    {
        _normalDmg = _myEntity.GetNormalDamage;
        _critcalDmg = _myEntity.GetCriticalDamage;
    }

    void Update()
    {
        if(_normalDmg != _myEntity.GetCriticalDamage) _normalDmg = _myEntity.GetNormalDamage;
        if(_critcalDmg != _myEntity.GetCriticalDamage) _critcalDmg = _myEntity.GetCriticalDamage;
    }

    public void Durability(float value)
    {
        _myEntity.WeaponDurability(value);
    }

    public void Repair()
    {
        _myEntity.Repair();
    }
}
