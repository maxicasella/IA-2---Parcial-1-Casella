using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quests : ScriptableObject
{
    [Header("Quest")]
    public string questName;
    public Texts _texts;
    public Items[] rewards;
    public int xpReward;

    public abstract Quests GetQuest();
}
