using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Recipe", menuName = "Recipe/Recipe")]
public class CraftRecipes_SO : CraftRecipes
{
    [Header("Recipe")]

    public RecipeType recipeType;
    public List<MaterialRequirement> materialsRequirements;
    public enum RecipeType
    {
        Repair,
        Craft
    }
    public override CraftRecipes GetRecipe()
    {
        return this;
    }
}
