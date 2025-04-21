using System.Collections.Generic;
using UnityEngine;

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

    public ItemType itemType;
    [Header("Stacking")]
    public int maxStackSize = 99;

    [Header("Value")]
    public float value;

    [Header("Rarity")]
    public string Rarity;

    [Header("FlatDamage")]
    public float flatDamage;

    [Header("DamageMult")]
    public float damageMult = 1;

    [Header("FlatDefense")]
    public float flatDefense;

    [Header("DefenseMult")]
    public float defenseMult = 1;

    [Header("FlatMaxHP")]
    public float flatMaxHP;

    [Header("MaxHPMult")]
    public float HPMult = 1;

    [Header("FlatPureDamage")]
    public float flatPureDamage;

    [Header("PureDamageMult")]
    public float pureDamageMult = 1;

    [Header("FlatMaxMana")]
    public float flatMaxMana;
    
    [Header("MaxManaMult")]
    public float maxManaMult = 1;

    [Header("DropRateIncrease")]
    public float dropRateIncrease;

    [Header("XP gain increase")]
    public float xpGainIncrease;

    [Header("GoldGainIncrease")]
    public float goldGainIncrease;
    [Header("FlatKnockback")]
    public float flatKnockbackIncrease;
    
    [Header("KnockbackMult")]
    public float knockbackMult = 1;

    [Header("KnockbackResistanceMult")]
    public float knockbackResistanceMult = 1;

    [Header("KnockbackResistanceFlat")]
    public float flatKnockbackResistance;

    [Header("AttackSpeedFlat")]
    public float attackSpeedFlat;
    [Header("AttackSpeedMult")]
    public float attackSpeedMult = 1;

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

}
