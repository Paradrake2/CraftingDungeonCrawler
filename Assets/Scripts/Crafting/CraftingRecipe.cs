using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RecipeSlotRequirement {
    public string requiredTag;
    public int quantityRequired;
}
public class PartVisual {
    public string partName;
    public Color tintColor;
}
[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Scriptable Objects/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeId;
    public string recipeName;
    public EquipmentSlot slot;
    //public int requiredSlots;
    public SlotCategory slotCategory;
    public List<RecipeSlotRequirement> requirements;
    public Sprite icon;
    public Sprite baseImage;
    public Transform visualPrefab;
    public string getRecipeId() {
        return recipeId;
    }
    public string getRecipeName() {
        return recipeName;
    }

    public EquipmentSlot GetEquipmentSlot() {
        return slot;
    }

    //public int getRequiredSlots() {
        //return requiredSlots;
    //}
    public Sprite getIcon() {
        return icon;
    }

    public List<RecipeSlotRequirement> getRequirements() {
        return requirements;
    }
}
