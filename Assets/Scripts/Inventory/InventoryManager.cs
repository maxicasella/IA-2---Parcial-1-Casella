using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public enum FilterType
{
    All,
    Tools,
    Miscellaneous,
    Consumibles
}
public class InventoryManager : MonoBehaviour
{
    public List<Slots> items = new List<Slots>();
    [SerializeField] GameObject _slotsHolder;
    GameObject[] _slots;

    [SerializeField] Button _toolsButton;
    [SerializeField] Button _miscellaneousButton;
    [SerializeField] Button _consumiblesButton;
    [SerializeField] Button _inventaryButton;

    private void Start()
     {
        _inventaryButton.onClick.AddListener(() => RefreshUI(FilterType.All));
        _toolsButton.onClick.AddListener(() => RefreshUI(FilterType.Tools));
        _miscellaneousButton.onClick.AddListener(() => RefreshUI(FilterType.Miscellaneous));
        _consumiblesButton.onClick.AddListener(() => RefreshUI(FilterType.Consumibles));

        _slots = new GameObject[_slotsHolder.transform.childCount];

        _slots = _slotsHolder.transform.Cast<Transform>()
            .Select(child => child.gameObject)  //IA 2 - Parcial 1
            .ToArray(); //IA 2 - Parcial 1

        RefreshUI();
     }

    public void RefreshUI(FilterType filterType = FilterType.All)
    {
        List<Slots> filteredItems;

        if (filterType == FilterType.All) filteredItems = items.ToList(); //IA 2 - Parcial 1
        else
        {
            //IA 2 - Parcial 1
            filteredItems = items.Where(slot =>
            {
                Items currentItem = slot.GetItem();
                switch (filterType)
                {
                    case FilterType.Tools:
                        return currentItem is Tools;
                    case FilterType.Miscellaneous:
                        return currentItem is Miscellaneous;
                    case FilterType.Consumibles:
                        return currentItem is Consumibles;
                    default:
                        return false;
                }
            }).ToList(); //IA 2 - Parcial 1
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            try
            {
                Slots slot = filteredItems.ElementAtOrDefault(i);

                if (slot != null)
                {
                    _slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    _slots[i].transform.GetChild(0).GetComponent<Image>().sprite = slot.GetItem().itemIcon;

                    if (slot.GetItem().isStackable) _slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = slot.GetQuantity().ToString();
                    else _slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                else
                {
                    _slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    _slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    _slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
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
        Slots slot = items.FirstOrDefault(s => s.GetItem() == item && s.GetItem().isStackable); //IA 2 - Parcial 1

        if (slot != null) slot.Add(value);
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

                var slotRemove = items.FirstOrDefault(slot => slot.GetItem() == item); //IA 2 - Parcial 1
                if (slotRemove != null) items.Remove(slotRemove);
            }
        }
        else return false;
        RefreshUI();
        return true;
    }

    public Slots ContainsSlots(Items item) //IA 2 - Parcial 1
    {
        return items.FirstOrDefault(slot => slot.GetItem()==item);
    }
}
