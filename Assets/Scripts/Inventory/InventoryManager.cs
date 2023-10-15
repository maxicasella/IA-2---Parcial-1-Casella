using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public List<Slots> items = new List<Slots>();
    [SerializeField] GameObject _slotsHolder;
    GameObject[] _slots;

    private void Start()
    {
        _slots = new GameObject[_slotsHolder.transform.childCount];
        for (int i = 0; i < _slotsHolder.transform.childCount; i++)
        {
            _slots[i] = _slotsHolder.transform.GetChild(i).gameObject;
        }
        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            try 
            {
                _slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                _slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if(items[i].GetItem().isStackable) _slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity().ToString();
                else _slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            catch
            {
                _slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                _slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                _slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
    public bool AddItem(Items item,int value)
    {
        Slots slot = ContainsSlots(item);
        if (slot != null && slot.GetItem().isStackable) slot.Add(value);
        else
        {
            if (items.Count < _slots.Length) items.Add(new Slots(item, value));
            else return false;
        }
        RefreshUI();
        return true;
    }

    public bool RemoveItem(Items item, int value)
    {
        Slots tempSlot = ContainsSlots(item);
        if (tempSlot != null)
        {
            if(tempSlot.GetQuantity() > 1) tempSlot.Subtract(value);
            else
            {
                Slots slotRemove = new Slots();
                foreach (Slots slot in items)
                {
                    if (slot.GetItem() == item)
                    {
                        slotRemove = slot;
                        break;
                    }
                }
                items.Remove(slotRemove);
            }
        }
        else return false;
 
        RefreshUI();
        return true;
    }

    public Slots ContainsSlots(Items item)
    {
        foreach (Slots slot in items)
        {
            if (slot.GetItem() == item) return slot;
        }
        return null;
    }
}
