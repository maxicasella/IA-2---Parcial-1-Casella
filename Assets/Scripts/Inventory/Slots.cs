using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slots
{
    [SerializeField] Items _item;
    [SerializeField] int _quantity;
    public string name;
    public Sprite imgIcon;
    public Texts texts;

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
}
