using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingFactory : MonoBehaviour
{
    public Equipment GenerateFromIngredients(List<Items> ingredients, CraftingRecipe recipeId) {
        var equipment = new Equipment();
        equipment.itemName = GenerateName(ingredients, recipeId);
        equipment.slot = recipeId.slot;
        equipment.modifiers = new List<StatModifier>();

        foreach (Items item in ingredients)
        {
            TryAddModifier(equipment, StatType.Damage, item.flatDamage, item.damageMult);
            TryAddModifier(equipment, StatType.Defense, item.flatDefense, item.defenseMult);
            TryAddModifier(equipment, StatType.MaxHealth, item.flatMaxHP, item.HPMult);
            TryAddModifier(equipment, StatType.Mana, item.flatMaxMana, item.maxManaMult);
            TryAddModifier(equipment, StatType.PureDamage, item.flatPureDamage, item.pureDamageMult);
            TryAddModifier(equipment, StatType.XPGain, item.xpGainIncrease, 0);
            TryAddModifier(equipment, StatType.GoldGain, item.goldGainIncrease, 0);
            TryAddModifier(equipment, StatType.DropRate, item.dropRateIncrease, 0);
            TryAddModifier(equipment, StatType.Knockback, item.flatKnockbackIncrease, item.knockbackMult);
            TryAddModifier(equipment, StatType.AttackSpeed, item.attackSpeedFlat, item.attackSpeedMult);
        }
        if (ingredients.Count != recipeId.requirements.Sum(r => r.quantityRequired)) {
            Debug.LogWarning("Invalid number of ingredients");
            return null;
        }
        return equipment;
    }
    public Equipment PreviewCraftedEquipment(List<Items> ingredients, CraftingRecipe recipe) {
        return GenerateFromIngredients(ingredients, recipe);
    }
    private void TryAddModifier(Equipment equipment, StatType stat, float flat, float mult) {
        if (flat == 0f && mult == 0f) return;

        var existing = equipment.modifiers.Find(m => m.statType == stat);
        if (existing != null) {
            existing.flatAmount += flat;
            existing.percentAmount += mult;
        } else {
            equipment.modifiers.Add(new StatModifier {
                statType = stat,
                flatAmount = flat,
                percentAmount = mult
            });
        }
    }

    private string GenerateName(List<Items> ingredients, CraftingRecipe recipe) {
        
        var baseName = ingredients[0].itemName;
        // if this is throwing an error it means there are no ingredients
        return $"{baseName} {recipe.getRecipeName()}";
    }
}
