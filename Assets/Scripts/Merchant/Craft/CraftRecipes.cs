using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CraftRecipes : ScriptableObject
{
    [Header("CraftRecipes")]
    public string recipeName;
    public Sprite imgIcon;
    public Texts texts;
    public Items craftObj;
    public Items[] materialsRequirement;
    public int[] valueMaterialsRequirement;

    public abstract CraftRecipes GetRecipe();
}
