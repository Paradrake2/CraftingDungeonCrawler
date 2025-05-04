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
    ManaRegeneration
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
    public int allowedAugments;
    public float equipmentLevel;
    public float equipmentXP;
}
