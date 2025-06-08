using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellComponentRecipes", menuName = "Scriptable Objects/SpellComponentRecipes")]
public class SpellComponentRecipes : ScriptableObject
{
    public string recipeName;
    public SpellComponent output;
    public List<RecipeSlotRequirement> requirements;
    public Sprite icon;
}
