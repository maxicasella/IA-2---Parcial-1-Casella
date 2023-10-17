using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MerchantSystem : MonoBehaviour
{
    [SerializeField] GameObject _merchantCanvas;
    [SerializeField] CharacterController _character;
    [SerializeField] List<CraftRecipes> _availableRecipes;
    List<Slots> recipes = new List<Slots>();
    [SerializeField] GameObject _slotsHolder;
    GameObject[] _slots;
    void Awake()
    {
        _merchantCanvas.SetActive(false);
    }

    void Start()
    {
        recipes = _availableRecipes.Select(recipe => {
            Slots newSlot = new Slots();
            newSlot.name = recipe.recipeName;
            newSlot.imgIcon = recipe.imgIcon; 
            newSlot.texts = recipe.texts;     
            return newSlot;
        }).ToList(); //IA 2 LINQ- Parcial 1

        _slots = _slotsHolder.transform.Cast<Transform>()
            .Select(child => child.gameObject)  //IA 2 LINQ - Parcial 1
            .ToArray(); //IA 2 LINQ- Parcial 1
    }
    public void EnableCanvas()
    {
        _character.enabled = false;
        _merchantCanvas.SetActive(true);
        RefreshUI();
    }
    public void DisabledCanvas()
    {
        _character.enabled = true;
        _merchantCanvas.SetActive(false);
    }
    public void RefreshUI(FilterType filterType = FilterType.All)
    {
        List<Slots> filteredItems = new List<Slots>();
        if (filterType == FilterType.All) filteredItems = recipes.ToList(); //IA 2 LINQ - Parcial 1

        for (int i = 0; i < _slots.Length; i++)
        {
            try
            {
                Slots slot = filteredItems.ElementAtOrDefault(i); //IA 2 LINQ - Parcial 1

                if (slot != null)
                {
                    _slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    _slots[i].transform.GetChild(0).GetComponent<Image>().sprite = slot.imgIcon;
                    _slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = slot.GetName();
                    _slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = slot.GetTxt();
                }
                else
                {
                    _slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    _slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    _slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    _slots[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            catch
            {
                Debug.Log("Refres UI failed");
            }
        }
    }
}