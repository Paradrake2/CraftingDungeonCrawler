using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> itemStacks = new List<InventoryItem>();
    public List<Equipment> ownedGear = new List<Equipment>();
    public static InventorySystem Instance;
    public void AddItem(string itemId, int amount){}
    public void RemoveItem(string itemId, int amount) {}
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

    public void AddEquipment(Equipment newEquip)
    {
        ownedGear.Add(newEquip);
        Debug.Log($"Added new equipment: {newEquip.itemName}");
    }
}
