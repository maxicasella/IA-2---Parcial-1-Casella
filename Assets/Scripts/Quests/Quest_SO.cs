using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "new Quest", menuName = "Quest/Quest")]
public class Quest_SO : Quests
{
    [Header("Quest")]

    public QuestType questType;
    public enum QuestType
    {
        Principal,
        Secondary
    }
    public override Quests GetQuest()
    {
        return this;
    }
    public bool MatchesRandomQuest(RandomQuest randomQuest)
    {
        return this.questType == randomQuest.questType && this.rewards.Any(reward => reward == randomQuest.reward); //IA 2 LINQ - Parcial 1
    }
}
