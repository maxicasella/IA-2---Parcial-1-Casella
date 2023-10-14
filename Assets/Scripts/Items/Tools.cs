using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Tool", menuName = "Item/Tool")]
public class Tools : Items
{
    [Header("Tool")]
    public ToolType toolType;
    public enum ToolType
    {
        Sword,
        Shield
    }

    public override Consumibles GetConsumible()
    {
        return null;
    }

    public override Items GetItem()
    {
        return this;
    }

    public override Miscellaneous GetMiscellaneous()
    {
        return null;
    }

    public override Tools GetTool()
    {
        return this;
    }
}
