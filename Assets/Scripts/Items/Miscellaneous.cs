using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Miscellaneous", menuName = "Item/Miscellaneous")]
public class Miscellaneous : Items
{
    [Header("Miscellaneous")]
    public int minValueAdded;
    public int maxValueAdded;

    public MiscellaneousType miscellaneousType;
    public enum MiscellaneousType
    {
        Wood,
        Rock,
        Metal,
        Crystal,
        Plants,
        Gold,
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
        return this;
    }

    public override Tools GetTool()
    {
        return null;
    }
}
