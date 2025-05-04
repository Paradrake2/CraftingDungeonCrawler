using System.Collections.Generic;
using UnityEngine;

public static class EquipmentFactory
{
    public static Equipment GenerateEquipmentFromItem(Items item)
    {
        Equipment newEquip = new Equipment();
        newEquip.itemName = item.itemName;
        newEquip.slot = DetermineSlot(item);
        newEquip.modifiers = new List<StatModifier>();

        void AddModifier(StatType stat, float flat, float percent)
        {
            if (flat != 0f || percent != 0f)
                newEquip.modifiers.Add(new StatModifier { statType = stat, flatAmount = flat, percentAmount = percent });
        }

        AddModifier(StatType.Damage, item.flatDamage, item.damageMult);
        AddModifier(StatType.Defense, item.flatDefense, item.defenseMult);
        AddModifier(StatType.MaxHealth, item.flatMaxHP, item.HPMult);
        AddModifier(StatType.PureDamage, item.flatPureDamage, item.pureDamageMult);
        AddModifier(StatType.Mana, item.flatMaxMana, item.maxManaMult);
        AddModifier(StatType.DropRate, item.dropRateIncrease, 0);
        AddModifier(StatType.XPGain, item.xpGainIncrease, 0);
        AddModifier(StatType.GoldGain, item.goldGainIncrease, 0);
        AddModifier(StatType.Knockback, item.flatKnockbackIncrease, item.knockbackMult);
        AddModifier(StatType.AttackSpeed, item.attackSpeedFlat, item.attackSpeedMult);

        return newEquip;
    }
    public static EquipmentSlot DetermineSlot(Items item)
    {
    if (item.tags != null)
    {
        if (System.Array.Exists(item.tags, t => t == "Helmet")) return EquipmentSlot.Helmet;
        if (System.Array.Exists(item.tags, t => t == "Boots")) return EquipmentSlot.Boots;
        if (System.Array.Exists(item.tags, t => t == "Chestplate")) return EquipmentSlot.Chestplate;
        if (System.Array.Exists(item.tags, t => t == "Weapon")) return EquipmentSlot.Weapon;
        if (System.Array.Exists(item.tags, t => t == "Accessory")) return EquipmentSlot.Accessory;
        /*
        if (System.Array.Exists(item.tags, t => t == "Accessory")) return EquipmentSlot.Accessory2;
        if (System.Array.Exists(item.tags, t => t == "Accessory")) return EquipmentSlot.Accessory3;
        if (System.Array.Exists(item.tags, t => t == "Accessory")) return EquipmentSlot.Accessory4;
        if (System.Array.Exists(item.tags, t => t == "Accessory")) return EquipmentSlot.Accessory5;
        */
    }
    return EquipmentSlot.Accessory; // fallback/default
}

}
