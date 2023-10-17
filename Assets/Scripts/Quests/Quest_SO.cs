using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
