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
    public static InventoryManager InventoryInstance { get; private set; }
    List<Slots> _originalItems;
    public List<Slots> items = new List<Slots>();
    GameObject[] _slots;

    [SerializeField] Character_Equipment _characterEquipment;
    [SerializeField] GameObject _slotsHolder;
    [SerializeField] Button _toolsButton;
    [SerializeField] Button _miscellaneousButton;
    [SerializeField] Button _consumiblesButton;
    [SerializeField] Button _inventaryButton;

    void Awake()
    {
        if (InventoryInstance) Destroy(gameObject);
        else InventoryInstance = this;
    }
    void Start()
    {
        _originalItems = new List<Slots>(items);
        _inventaryButton.onClick.AddListener(() => RefreshUI(FilterType.All));
        _toolsButton.onClick.AddListener(() => RefreshUI(FilterType.Tools));
        _miscellaneousButton.onClick.AddListener(() => RefreshUI(FilterType.Miscellaneous));
        _consumiblesButton.onClick.AddListener(() => RefreshUI(FilterType.Consumibles));

        _slots = new GameObject[_slotsHolder.transform.childCount];

        _slots = _slotsHolder.transform.Cast<Transform>()
            .Select(child => child.gameObject)  //IA 2 LINQ - Parcial 1
            .ToArray(); //IA 2 LINQ- Parcial 1

        RefreshUI();
    }

    public void RefreshUI(FilterType filterType = FilterType.All)
    {
        List<Slots> filteredItems;

        if (filterType == FilterType.All) filteredItems = items.ToList(); //IA 2 LINQ - Parcial 1
        else
        {
            //IA 2 LINQ - Parcial 1
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
            }).ToList(); //IA 2 LINQ - Parcial 1
        }

        if (filteredItems.Any()) //IA 2 LINQ - Parcial 1
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                try
                {
                    Slots slot = filteredItems.ElementAtOrDefault(i); //IA 2 LINQ- Parcial 1

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
        _characterEquipment.GetInventoryWeapons();
    }

    public void AddItem(Items item, int value)
    {
        Slots slot = items.Concat(_originalItems).FirstOrDefault(s => s.GetItem() == item); //IA 2 LINQ- Parcial 1

        if (slot != null)
        {
            if (item.isStackable) slot.Add(value);
        }
        else
        {
            if (items.Count < _slots.Length) items.Add(new Slots(item, value));
            _originalItems.Add(new Slots(item, value));
        }
        RefreshUI();
    }

    public bool RemoveItem(Items item, int value)
    {
        Slots tempSlot = ContainsSlots(item);

        if (tempSlot != null)
        {
            tempSlot.Subtract(value);

            if (tempSlot.GetQuantity() <= 0)
            {
                var slotRemove = items.FirstOrDefault(slot => slot.GetItem() == item);  //IA 2 LINQ - Parcial 1
                if (slotRemove != null)
                    items.Remove(slotRemove);
            }
            ConsumeMaterials(new Items[] { item }, new int[] { value });  
            RefreshUI();
            return true;
        }
        return false;
    }

    public Slots ContainsSlots(Items item) //IA 2 LINQ - Parcial 1
    {
        return items.FirstOrDefault(slot => slot.GetItem()==item);
    }

    public void OrderListDescending()
    {
        items = items.Aggregate(new List<Slots>(), (sortedList, nextItem) =>
        {
            int index = sortedList.FindIndex(slot => slot.GetQuantity() <= nextItem.GetQuantity());
            if (index == -1)
            {
                sortedList.Add(nextItem);
            }
            else
            {
                sortedList.Insert(index, nextItem);
            }
            return sortedList;
        }); //IA2-P1

        RefreshUI();
    }

    public void OriginalListOrder()
    {
        items = new List<Slots>(_originalItems);

        RefreshUI();
    }
    public int GetItemQuantity(Items item)
    {
        int totalQuantity = items
       .Where(slot => slot.GetItem() == item) //IA 2 LINQ - Parcial 1
       .Aggregate(0, (acc, slot) => acc + slot.GetQuantity());//IA2-P1

        return totalQuantity;
    }
    public bool HasItem(Items item, int quantity)
    {
        return items.Any(slot => slot.GetItem() == item && slot.GetQuantity() >= quantity);//IA 2 LINQ - Parcial 1

    }
  public void ConsumeMaterials(Items[] materials, int[] values)
 {
        if (materials != null && values != null && materials.Length == values.Length)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                Items material = materials[i];
                int value = values[i];

                if (material != null)
                {
                    Slots slot = items.Concat(_originalItems).FirstOrDefault(s => s.GetItem() == material); //IA 2 LINQ - Parcial 1

                    if (slot != null)
                    {
                        slot.Subtract(value);
                        if (slot.GetQuantity() <= 0)
                        {
                            var slotRemove = items.FirstOrDefault(s => s == slot); //IA 2 LINQ - Parcial 1
                            if (slotRemove != null)
                            {
                                items.Remove(slotRemove);
                                _originalItems.Remove(slot);
                            }
                         }
                    }
                }
            }
        }
        RefreshUI();
    }
    public bool HasMaterialsForRecipe(Slots recipe)
    {
        if (recipe.materialsRequirement != null && recipe.materialsRequirement.Length > 0)
        {
            return !recipe.materialsRequirement
                .Select((material, index) => new { Material = material, Amount = recipe.valueMaterialsRequirement[index] })
                .Any(materialInfo => items
                    .Where(slot => slot.GetItem() == materialInfo.Material)
                    .Sum(slot => slot.GetQuantity()) < materialInfo.Amount);
        }
        else return true;
    }
}
