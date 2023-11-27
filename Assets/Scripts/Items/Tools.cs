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

    [SerializeField] protected int _baseDamage;
    [SerializeField] protected int _criticalDamage;

    public float _maxDurability;
    public float _actualDurability;

    //Getters
    public int NormalDamage { get { return _baseDamage; } }
    public float Durability { get { return _actualDurability; } set { _actualDurability -= value; } }
    public float RepairWeapon { set { _actualDurability += value; } }
    public int CriticalDamage { get { return _criticalDamage; } }

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
