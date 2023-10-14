using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumible", menuName = "Item/Consumible")]
public class Consumibles : Items
{
    [Header("Consumibles")]
    public float valueAdded;

    public ConsumiblesType consumiblesType;
    public enum ConsumiblesType
    {
       Life,
       Stamina,
    }
    public override Consumibles GetConsumible()
    {
        return this;
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
        return null;
    }
}
