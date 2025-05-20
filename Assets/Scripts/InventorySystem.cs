using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> itemStacks = new List<InventoryItem>();
    public List<Equipment> ownedGear = new List<Equipment>();
    public List<Augment> ownedAugments = new List<Augment>();
    public HashSet<String> discoveredRefinedItems = new();
    public static InventorySystem Instance;
    public void RemoveItem(string itemId, int amount) {
        var stack = itemStacks.Find(i => i.itemId == itemId);
        if (stack != null) {
            stack.quantity -= amount;
            if (stack.quantity <= 0) {
                itemStacks.Remove(stack);
            }
        }
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
    public void AddAugment(Augment newAugment) {
        ownedAugments.Add(newAugment);
        Debug.Log($"Added new augment: {newAugment.augmentName}");
    }
    public void RemoveAugment(Augment augment) {
        ownedAugments.Remove(augment);
        Debug.Log($"Removed {augment}");
    }
    public void AddEquipment(Equipment newEquip)
    {
        ownedGear.Add(newEquip);
        Debug.Log($"Added new equipment: {newEquip.itemName}");
    }
    public void AddItem(string itemId, int amount) {
        InventoryItem existing = itemStacks.Find(i => i.itemId == itemId);

        if (existing != null) {
            existing.quantity += amount;
        } else {
            itemStacks.Add(new InventoryItem {itemId = itemId, quantity = amount});
        }
        Debug.Log($"Added{amount} of {itemId} to inventory.");
    }
    public bool HasItem(string itemId, int amount) {
        var stack = itemStacks.Find(i => i.itemId == itemId);
        return stack != null && stack.quantity >= amount;
    }
    public Augment GetAugment(string augmentName) {
        foreach (var augment in ownedAugments) {
            if (augment.augmentName == augmentName) {
                return augment;
            }
        }
        return null;
    }
    public int GetQuantity(string requestedItemName) {
        foreach (var item in itemStacks) {
            if (item.itemId == requestedItemName) {
                return item.quantity;
            }
        }
        return 0;
    }

}
