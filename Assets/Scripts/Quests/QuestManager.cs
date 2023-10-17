using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public List<Quest_SO> _quests;
    public List<Quests> _rewards;
    RandomQuest _selectedQuest;

    public void GetRandomQuest() 
    {
        RandomQuest selectQuest = null;

        var randomQuest = _quests.SelectMany(quest => quest.rewards, (quest, reward) => new { Quest = quest, Reward = reward })
            .Where(data => data.Quest._texts != null)
            .Select(data => new RandomQuest(data.Quest.questType, data.Reward, data.Quest._texts))
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
}
