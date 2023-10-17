using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public event Action<RandomQuest> CompletedQuest;
    public List<Quest_SO> _quests;
    public List<Quests> _rewards;
    RandomQuest _selectedQuest;

    public RandomQuest ActualQuest { get { return _selectedQuest; } }
    public void GetRandomQuest() 
    {
        RandomQuest selectQuest = null;

        var randomQuest = _quests.SelectMany(quest => quest.rewards, (quest, reward) => new { Quest = quest, Reward = reward })
            .Where(data => data.Quest.texts != null)
            .Select(data => new RandomQuest(data.Quest.questType, data.Reward, data.Quest.texts))
            .ToList(); //IA 2 LINQ - Parcial 1

        if (randomQuest.Count > 0) selectQuest = randomQuest[UnityEngine.Random.Range(0, randomQuest.Count)];
        SetQuest(selectQuest);
    }
    void SetQuest(RandomQuest quest)
    {
        _selectedQuest = quest;
    }
    public RandomQuest GetQuest()
    {
        GetRandomQuest();
        return _selectedQuest;
    }
    public void QuestCompleted(RandomQuest completedQuest)
    {
        if (CompletedQuest != null)
        {
            CompletedQuest(completedQuest);
            Debug.Log("CompletedQuest event called.");
            _selectedQuest = null;
        }
    }
    public Quest_SO GetCompletedQuest(RandomQuest quest)
    {
        return _quests.FirstOrDefault(questSO => questSO.MatchesRandomQuest(quest)); //IA 2 LINQ - Parcial 1
    }
}
