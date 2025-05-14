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
public enum MaterialRoleType {
    Plate,
    Binding,
    Blade,
    Guard,
    Handle,
    Accessory,
    Gem
}
[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Scriptable Objects/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeId;
    [System.NonSerialized] public SpriteGenerator spriteGenerator;
    public string recipeName;
    public EquipmentSlot slot;
    //public int requiredSlots;
    public SlotCategory slotCategory;
    public List<RecipeSlotRequirement> requirements;
    public List<EquipmentPartSprite> parts;
    public Sprite icon;
    public Sprite baseImage;
    public MaterialVisualData[] visualLayers;
    public GameObject visualPrefab;
    public int augmentSlots;
    public int requiredLevel;
    public string getRecipeId() {
        return recipeId;
    }
    public string getRecipeName() {
        return recipeName;
    }

    public EquipmentSlot GetEquipmentSlot() {
        return slot;
    }
    public Sprite getIcon() {
        return icon;
    }

    public List<RecipeSlotRequirement> getRequirements() {
        return requirements;
    }
    public int getAugmentSlots() {
        return augmentSlots;
    }

    public void GenerateSprite(GameObject visualPrefab, List<Items> ingredients) {
        spriteGenerator.GenerateIcon(visualPrefab, ingredients);
    }
}
