using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Scriptable Objects/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeId;
    public string recipeName;
    public EquipmentSlot slot;
    public int requiredSlots;
    public Sprite icon;

    public string getRecipeId() {
        return recipeId;
    }
    public string getRecipeName() {
        return recipeName;
    }

    public EquipmentSlot GetEquipmentSlot() {
        return slot;
    }

    public int getRequiredSlots() {
        return requiredSlots;
    }
    public Sprite getIcon() {
        return icon;
    }
}
