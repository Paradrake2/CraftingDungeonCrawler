using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Dictionary<EquipmentSlot, Equipment> equippedItems = new();

    // Base stats
    public int Level = 1;
    public float XP = 0;
    public float BaseHealth = 100;
    public float BaseDefense = 0;
    public float BaseDamage = 1;
    public float PureDamage = 0;
    public float BaseMana = 100;
    public float BaseKnockback = 1;
    public float BaseAttackSpeed = 0.5f;
    public float BaseDropRate = 0.5f;
    public float BaseXPGain = 1f;
    public float BaseGoldGain = 1f;
    public float BaseRegeneration = 0f;
    public float BaseManaRegeneration = 1f;
    public float CurrentHealth;
    public float CurrentDefense => CalculateStat(StatType.Defense);
    public float CurrentDamage => CalculateStat(StatType.Damage) + PureDamage;
    public float CurrentMaxHealth => CalculateStat(StatType.MaxHealth);
    public float CurrentMaxMana => CalculateStat(StatType.Mana);
    public float CurrentMagicDamage => CalculateStat(StatType.MagicDamage);
    public float CurrentKnockback => CalculateStat(StatType.Knockback);
    public float CurrentAttackSpeed => CalculateStat(StatType.AttackSpeed);
    public float CurrentRegeneration => CalculateStat(StatType.Regeneration);
    public float CurrentManaRegeneration => CalculateStat(StatType.ManaRegeneration);
    public float CurrentDropRate => CalculateStat(StatType.DropRate);
    public float CurrentXPGain => CalculateStat(StatType.XPGain);
    public float XpToNextLevel => Level * 100;

    void Start()
    {
        CurrentHealth = CurrentMaxHealth;
    }

    public void EquipItem(Equipment newItem) {
        equippedItems[newItem.slot] = newItem;
    }

    float CalculateStat(StatType type) {
        float flat = 0f;
        float percent = 1f;

        foreach(var kvp in equippedItems) {
            var item = kvp.Value;
            foreach(var mod in item.modifiers) {
                if (mod.statType == type) {
                    flat += mod.flatAmount;
                    percent += mod.percentAmount;
                }
            }
        }
        float baseValue = (float)type switch {
            (float)StatType.MaxHealth => BaseHealth,
            (float)StatType.Defense => BaseDefense,
            (float)StatType.Damage => BaseDamage,
            (float)StatType.DropRate => BaseDropRate,
            (float)StatType.Mana => BaseMana,
            (float)StatType.Knockback => BaseKnockback,
            (float)StatType.GoldGain => BaseGoldGain,
            (float)StatType.Regeneration => BaseRegeneration,
            (float)StatType.XPGain => BaseXPGain,
            (float)StatType.ManaRegeneration => BaseManaRegeneration,
            _ => 0
        };

        return Mathf.RoundToInt((baseValue + flat) * percent);

    }
    public void GainXP(float amount) {
        XP += amount;
        while(XP >= XpToNextLevel) {
            XP -= XpToNextLevel;
            LevelUp();
        }
    }
    void LevelUp() {
        Level++;
        BaseHealth += 10;
        BaseDefense += 1;
        CurrentHealth = CurrentMaxHealth;

        Debug.Log("Leveled up to level " + Level);
    }
}
