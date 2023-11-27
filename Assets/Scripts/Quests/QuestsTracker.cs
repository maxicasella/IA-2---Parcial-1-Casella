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
    bool _questChecked;
    void Start()
    {
        _completedQuest = false;
        _questChecked = false;
       _questManager.CompletedQuest += OnQuestCompleted;
    }

    void Update()
    {
        if (!_completedQuest && _questManager.ActualQuest != null && !_questChecked)
        {
            _questChecked = true;
            CheckQuest();
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
            _questManager.ChangeQuest();
            foreach (var reward in completedQuest.rewards)
            {
                InventoryManager.InventoryInstance.AddItem(reward, completedQuest.rewardsValue);
            }
        }
        else return;
        StartCoroutine(UpdateText());
    }
    private bool CheckQuestRequirements(Quest_SO quest)
    {
        Items[] questRequirements = quest.questRequirements;
        int[] valueRequirements = quest.valueQuestRequirements;

        if (questRequirements == null || valueRequirements == null) return true;

        bool requirementsOk = questRequirements
            .Zip(valueRequirements, (item, value) => new { Item = item, Value = value }) // IA 2 LINQ - Parcial 1
             .Aggregate(true, (result, pair) =>
            {
                int quantity = InventoryManager.InventoryInstance.GetItemQuantity(pair.Item);
                return result && (quantity >= pair.Value);
            }); //IA2-P1

        return requirementsOk;
    }

    void CheckQuest()
    {
        Quest_SO matchingQuest = _questManager._quests.FirstOrDefault(quest => quest.MatchesRandomQuest(_questManager.ActualQuest));

        if (matchingQuest != null)
        {
            _questManager.QuestCompleted(_questManager.ActualQuest);
            _questChecked = false;
        }
    }

    IEnumerator UpdateText() //IA 2 - Parcial 1
    {
        _txt.text = "¡Logro cumplido!";
        _txt.color = Color.green;
        yield return new WaitForSeconds(_timeMessage);
        _txt.color = Color.white;
        _txt.text = "Sin misiones activas";
        _completedQuest = false;
    }
}
