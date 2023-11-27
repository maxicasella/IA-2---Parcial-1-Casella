using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Slots
{
    [SerializeField] Items _item;
    [SerializeField] int _quantity;
    public string name;
    public Sprite imgIcon;
    public Texts texts;
    public Items[] materialsRequirement;
    public int[] valueMaterialsRequirement;
    public Items craftObj;

    public Slots()
    {
        _item = null;
        _quantity = 0;
        name= null;
    }

    public Slots(Items item, int quantity)
    {
        _item = item;
        _quantity = quantity;
    }
    public Items GetItem()
    {
        return _item;
    }
    public int GetQuantity()
    {
        return _quantity;
    }
    public void Add(int value)
    {
        _quantity += value;
    }
    public void Subtract(int value)
    {
        _quantity -= value;
    }

    public string GetName()
    {
        return name;
    }
    public string GetTxt()
    {
        return texts.textsArray[0].ToString();
    }
    public bool HasMaterialsForRecipe(Slots recipe, List<Slots> originalItems)
    {
        bool hasMaterials = !recipe.materialsRequirement
      .Select((material, index) => new { Material = material, Amount = recipe.valueMaterialsRequirement[index] }) //IA 2 LINQ - Parcial 1
      .Aggregate(true, (result, materialInfo) =>
      {
          int totalQuantity = originalItems
              .Where(slot => slot.GetItem() == materialInfo.Material)
              .Sum(slot => slot.GetQuantity());

          return result && totalQuantity >= materialInfo.Amount;
      }); //IA2-P1

        return hasMaterials;
    }
}
