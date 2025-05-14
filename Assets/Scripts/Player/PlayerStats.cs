using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public Dictionary<EquipmentSlot, Equipment> equippedItems = new();
    public Equipment[] accessorySlots = new Equipment[5];
    public static PlayerStats Instance;
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
    public float DashDistance = 2f;
    public int DashNumber = 2;
    public bool isDashing = false;
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
    public float CalculateDashDistance => CalculateStat(StatType.DashDistance);
    public int CalculateDashNumber => Mathf.FloorToInt(CalculateStat(StatType.DashNumber));
    public float XpToNextLevel => Level * 1000;
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        CurrentHealth = CurrentMaxHealth;
    }

    public void EquipItem(Equipment newItem) {
        if (newItem == null) return;
        if (newItem.slot == EquipmentSlot.Accessory) {
            Debug.LogWarning("Is accessory");
            EquipAccessory(newItem);
        } else {
            EquipMainGear(newItem);
        }
    }
    private void EquipMainGear(Equipment newItem) {
        equippedItems[newItem.slot] = newItem;
        Debug.Log($"Equipped {newItem.itemName} into {newItem.slot}");
    }
    private void EquipAccessory(Equipment newAccessory) {
        foreach (var equipped in accessorySlots) {
            if (equipped == newAccessory) {
                Debug.Log("Already equipped");
                return;
            }
        }
        for (int i = 0; i < accessorySlots.Length; i++) {
            if (accessorySlots[i] == null || string.IsNullOrEmpty(accessorySlots[i].itemName)) {
                accessorySlots[i] = newAccessory;
                Debug.Log($"Equipped {newAccessory.itemName} into slot {i}");
                return;
            }
        }
    }

    public Equipment GetEquippedItem(EquipmentSlot slot) {
        equippedItems.TryGetValue(slot, out Equipment item);
        return item;
    }

    public Equipment GetAccessoryAt(int index) {
        if (index >= 0 && index < accessorySlots.Length) return accessorySlots[index];
        else {return null;}
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
        foreach(var accessory in accessorySlots) {
            if (accessory == null) continue;
            foreach(var mod in accessory.modifiers) {
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
            (float)StatType.DashDistance => DashDistance,
            (float)StatType.DashNumber => DashNumber,
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
        BaseDefense += 0.5f;
        
        Debug.Log("Leveled up to level " + Level);
    }
    public float GetStat(StatType statType) {
        return CalculateStat(statType);
    }
}
