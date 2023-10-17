using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class RandomQuest
{
    public Quest_SO.QuestType questType;
    public Items reward;
    public Texts texts;

    public RandomQuest(Quest_SO.QuestType type, Items rwrd, Texts txt)
    {
        questType = type;
        reward = rwrd;
        texts = txt;
    }
}
