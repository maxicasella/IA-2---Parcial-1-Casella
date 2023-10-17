using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quests : ScriptableObject
{
    [Header("Quest")]
    public string questName;
    public Texts texts;
    public int rewardsValue;
    public Items[] rewards;
    public Items[] questRequirements;
    public int[] valueQuestRequirements;
    public bool questCompleted { get; set; }

    public abstract Quests GetQuest();
}
