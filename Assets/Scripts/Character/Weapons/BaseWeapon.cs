using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Normal,
    Rare,
    Legendary
}
public class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected float _baseDamage;
    [SerializeField] protected float _criticalDamage;
    [SerializeField] float _maxDurability;
 
    protected float _actualDurability;

    //Getters
    public float NormalDamage { get { return _baseDamage; } }
    public float Durability { get { return _actualDurability; } }
    public float CriticalDamage { get { return _criticalDamage; } }

    BaseWeapon _actualWeapon;

    void Awake()
    {
        _actualDurability = _maxDurability;   
    }
}
