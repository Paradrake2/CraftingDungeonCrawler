using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythical,
    Gauranteed
}
public enum ItemType {
    Material,
    MonsterDrop,
    CraftingIngredient,
    Equipment,
    Consumable,
    KeyItem
}
[CreateAssetMenu(menuName = "Item/Item Data")]
public class Items : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    [Header("MaterialColor")]
    public Color color;
    [Header("MaterialSpriteList")]
    public Sprite[] sprites;

    public ItemType itemType;
    [Header("Stacking")]
    public int maxStackSize = 99;

    [Header("Value")]
    public float value;

    [Header("Rarity")]
    public ItemRarity Rarity;

    [Header("FlatDamage")]
    public float flatDamage;

    [Header("DamageMult")]
    public float damageMult;

    [Header("FlatDefense")]
    public float flatDefense;

    [Header("DefenseMult")]
    public float defenseMult;

    [Header("FlatMaxHP")]
    public float flatMaxHP;

    [Header("MaxHPMult")]
    public float HPMult;

    [Header("FlatPureDamage")]
    public float flatPureDamage;

    [Header("PureDamageMult")]
    public float pureDamageMult;

    [Header("FlatMaxMana")]
    public float flatMaxMana;
    
    [Header("MaxManaMult")]
    public float maxManaMult;

    [Header("DropRateIncrease")]
    public float dropRateIncrease;

    [Header("XP gain increase")]
    public float xpGainIncrease;

    [Header("GoldGainIncrease")]
    public float goldGainIncrease;
    [Header("FlatKnockback")]
    public float flatKnockbackIncrease;
    
    [Header("KnockbackMult")]
    public float knockbackMult;

    [Header("KnockbackResistanceMult")]
    public float knockbackResistanceMult;

    [Header("KnockbackResistanceFlat")]
    public float flatKnockbackResistance;

    [Header("AttackSpeedFlat")]
    public float attackSpeedFlat;
    [Header("AttackSpeedMult")]
    public float attackSpeedMult;

    [Header("Drop Rate")]
    public float dropRate;

    [Header("Tags")]
    public string[] tags;

    [Header("Internal ID")]
    public string ID;

    [Header("Crafting material?")]
    public bool isCraftingMaterial;

    [Header("Can be crafted?")]
    public bool isCraftable;
    public List<Items> requiredMaterials;
    public List<int> requiredQuantities;

    [Header("CoreTier")]
    public int coreTier;
    [Header("PowderAmount")]
    public float powderAmount;
}
