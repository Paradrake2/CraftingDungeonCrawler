using System.Collections.Generic;
using UnityEngine;

public class EnhanceItem : MonoBehaviour
{
    public EnhanceUIManager enhanceUIManager;
    public void EnhanceMaterial(Items item)
    {
        float successChance = GetSuccessChance(item);
        int powderCost = GetPowderCost(item);
        if (powderCost > PowderInventory.Instance.totalPowder)
        {
            enhanceUIManager.NotEnoughPowder();
            return;
        }
        PowderInventory.Instance.RemovePowder(powderCost);
        if (Random.value <= successChance)
        {
            // give enhanced item
            Items enhanced = GenerateNewItem(item);
            InventorySystem.Instance.AddItem(enhanced.ID, 1);
        }
        else
        {
            enhanceUIManager.FailedEnhance();
        }
        InventorySystem.Instance.RemoveItem(item.ID, 1);
        return;
    }
    public float GetSuccessChance(Items item)
    {
        return Mathf.Max(0.7f - item.enhancedNum * 0.1f, 0.1f);
    }
    private Items GenerateNewItem(Items original)
    {
        string baseName = original.itemName;
        int plusIndexN = baseName.LastIndexOf(" +");
        if (plusIndexN != -1) baseName = baseName.Substring(0, plusIndexN);

        string baseId = original.ID;
        int plusIndexI = baseId.LastIndexOf(" +");
        if (plusIndexI != -1) baseId = baseId.Substring(0, plusIndexI);
        string newID = baseId + " +" + (original.enhancedNum + 1);
        if (ItemRegistry.Instance.GetItemById(newID) != null)
            return ItemRegistry.Instance.GetItemById(newID);
        Items enhance = new Items
        {
            itemName = baseName + " +" + (original.enhancedNum + 1),
            icon = original.icon,
            tags = original.tags,
            enhancedNum = original.enhancedNum + 1,
            Rarity = original.Rarity,
            description = original.description,
            dashNumber = original.dashNumber,
            dashDistance = original.dashDistance,
            ID = newID,
            value = original.value + 1,
            color = original.color
        };
        if (original.flatDamage != 0) enhance.flatDamage = original.flatDamage + 1;
        if (original.damageMult != 0) enhance.damageMult = original.damageMult + 0.01f;
        if (original.flatDefense != 0) enhance.flatDefense = original.flatDefense + 1;
        if (original.defenseMult != 0) enhance.defenseMult = original.defenseMult + 0.01f;
        if (original.flatMaxHP != 0) enhance.flatMaxHP = original.flatMaxHP + 10f;
        if (original.HPMult != 0) enhance.HPMult = original.HPMult + 0.01f;
        if (original.flatPureDamage != 0) enhance.flatPureDamage = original.flatPureDamage + 1;
        if (original.pureDamageMult != 0) enhance.pureDamageMult = original.pureDamageMult + 0.01f;
        if (original.flatMaxMana != 0) enhance.flatMaxMana = original.flatMaxMana + 1f;
        if (original.maxManaMult != 0) enhance.maxManaMult = original.maxManaMult + 0.01f;

        // add to InventorySystem and ItemRegistery
        ItemRegistry.Instance.AddItem(enhance);
        return enhance;
    }
    public int GetPowderCost(Items item)
    {
        return (item.enhancedNum * 100) + 100;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
