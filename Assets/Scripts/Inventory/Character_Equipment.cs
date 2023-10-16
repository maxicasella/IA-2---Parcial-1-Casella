using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character_Equipment : MonoBehaviour
{
    
    List<Tools> _equippedWeapons = new List<Tools>();
    [SerializeField] InventoryManager _inventoryManager;

    public void Equip(Tools tool)
    {
        if(!_equippedWeapons.Contains(tool)) _equippedWeapons.Add(tool);
    }

    public void Unequip(Tools tools)
    {
        _equippedWeapons.Remove(tools);
    }

    public List<Tools> GetWeapons(Tools.ToolType weaponType) //IA 2 LINQ - Parcial 1
    {
        return _equippedWeapons.Where(tool => tool.toolType == weaponType).ToList();
    }

    public List<Tools> GetWeaponsByRarity(Tools.ToolType weaponType, int n) //IA 2 LINQ - Parcial 1
    {
        return _equippedWeapons.Where(tool => tool.toolType == weaponType).OrderByDescending(tool => tool.rarity).Take(n).ToList();
    }

    public Tools GetEquippedWeapon(Tools.ToolType weaponType) //IA 2 LINQ - Parcial 1
    {
        return _equippedWeapons.FirstOrDefault(tool => tool.toolType == weaponType);
    }
    public void GetInventoryWeapons()//IA 2 LINQ - Parcial 1
    {
        List<Tools> weaponsToAdd = _inventoryManager.items
       .Where(slot => slot.GetItem() is Tools)
       .Select(slot => slot.GetItem().GetTool())
       .ToList();

        _equippedWeapons.AddRange(weaponsToAdd);
    }
}
