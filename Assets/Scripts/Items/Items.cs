using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items: ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public Sprite itemIcon;

    public abstract Items GetItem();
    public abstract Tools GetTool();
    public abstract Miscellaneous GetMiscellaneous();
    public abstract Consumibles GetConsumible();
}
