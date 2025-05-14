using UnityEngine;
using System.Collections.Generic;
public enum EquipmentSlot {
    Boots,
    Leggings,
    Chestplate,
    Gauntlets,
    Helmet,
    Weapon,
    Accessory
}
public enum SlotCategory {
    Armor,
    Weapon,
    Accessory
}
public enum StatType {
    MaxHealth,
    Defense,
    Damage,
    DropRate,
    GoldGain,
    Regeneration,
    XPGain,
    PureDamage,
    Mana,
    MagicDamage,
    Knockback,
    AttackSpeed,
    ManaRegeneration,
    DashDistance,
    DashNumber
}

[System.Serializable]
public class StatModifier {
    public StatType statType;
    public float flatAmount = 0f;
    public float percentAmount = 0f;
}

[System.Serializable]
public class Equipment
{
    public string itemName;
    public Sprite icon;
    public SlotCategory category;
    public EquipmentSlot slot;
    public List<StatModifier> modifiers = new List<StatModifier>();
    public List<Augment> appliedAugments = new();
    public int augmentSlotNumber;
    public int allowedAugments;
    public int equipmentLevel = 1;
    public float equipmentXP;
    /*
    public int GetXpToNextLevel(Equipment equip) => equip.equipmentLevel * 100;
    public void AddXP(float xp, Equipment equipment) {
        equipmentXP += xp;
        if (equipmentXP >= equipmentLevel*100) {
            equipmentXP = 0;
            equipmentLevel++;
            EquipmentLevelUp(equipment);
        }
    }
    public int XpNeeded(Equipment equipment) {
        return equipment.equipmentLevel * 200;
    }
    public void EnhanceWithPowder(float powderXP, Equipment equipment) {
        AddXP(powderXP, equipment);
    }

    public void EquipmentLevelUp(Equipment equipment) {
        foreach (var mod in equipment.modifiers) {
            if (mod.flatAmount > 0f) {
                mod.percentAmount += 0.01f;
                Debug.Log($"Boosted {mod.statType} + 1% on {equipment.itemName}");
            }
        }
    }
    */
}
