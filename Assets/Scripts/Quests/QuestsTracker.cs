using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;

public class QuestsTracker : MonoBehaviour
{
    [SerializeField] QuestManager _questManager;
    [SerializeField] TextMeshProUGUI _txt;
    [SerializeField] float _timeMessage;

    bool _completedQuest;
    void Start()
    {
       _questManager.CompletedQuest += OnQuestCompleted;
    }

    void Update()
    {
        if(!_completedQuest && _questManager.ActualQuest != null)
        {
            Quest_SO matchingQuest = _questManager._quests.FirstOrDefault(quest => quest.MatchesRandomQuest(_questManager.ActualQuest)); //IA 2 LINQ - Parcial 1
            
            if (matchingQuest!=null)
            {
                _questManager.QuestCompleted(_questManager.ActualQuest);
            }
        }
    }
    public void OnQuestCompleted(RandomQuest quest)
    {
        Debug.Log("OnQuestCompleted called!");
        Quest_SO completedQuest = _questManager.GetCompletedQuest(quest);

        if (completedQuest == null) return;
        bool requirementsOk = CheckQuestRequirements(completedQuest);

        if (requirementsOk)
        {
            completedQuest.questCompleted = true;
            _completedQuest = true;
            _txt.text = "¡Logro cumplido!";
            _txt.color = Color.green;
            foreach (var reward in completedQuest.rewards)
            {
                InventoryManager.InventoryInstance.AddItem(reward, completedQuest.rewardsValue);
            }
            StartCoroutine(UpdateText());
        }
    }
    private bool CheckQuestRequirements(Quest_SO quest)
    {
        Items[] questRequirements = quest.questRequirements;
        int[] valueRequirements = quest.valueQuestRequirements;

        if (questRequirements == null || valueRequirements == null) return true;

        var requirementPairs = questRequirements.Zip(valueRequirements, (item, value) => new { Item = item, Value = value }); //IA 2 LINQ - Parcial 1

        return requirementPairs.All(pair => InventoryManager.InventoryInstance.GetItemQuantity(pair.Item) >= pair.Value);//IA 2 LINQ - Parcial 1
    }
    IEnumerator UpdateText() //IA 2 - Parcial 1
    {
        yield return new WaitForSeconds(_timeMessage);
        _txt.text = "Sin misiones activas";
        _completedQuest = false;
    }
}
