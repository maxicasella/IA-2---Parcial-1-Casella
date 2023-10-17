using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Tool", menuName = "Item/Tool")]
public class Tools : Items
{
    [Header("Tool")]
    public ToolType toolType;
    public enum ToolType
    {
        Sword,
        Shield
    }

    public Rarity rarity;
    public enum Rarity
    {
        Normal,
        Rare,
        Legendary
    }

    [SerializeField] protected float _baseDamage;
    [SerializeField] protected float _criticalDamage;

    public float _maxDurability;
    public float _actualDurability;

    //Getters
    public float NormalDamage { get { return _baseDamage; } }
    public float Durability { get { return _actualDurability; } set { _actualDurability -= value; } }
    public float RepairWeapon { set { _actualDurability += value; } }
    public float CriticalDamage { get { return _criticalDamage; } }

    void Awake()
    {
        _actualDurability = _maxDurability;
    }

    public override Consumibles GetConsumible()
    {
        return null;
    }

    public override Items GetItem()
    {
        return this;
    }

    public override Miscellaneous GetMiscellaneous()
    {
        return null;
    }

    public override Tools GetTool()
    {
        return this;
    }
    public void Repair()
    {
        if (_actualDurability < _maxDurability) _actualDurability = _maxDurability;
    }
}
