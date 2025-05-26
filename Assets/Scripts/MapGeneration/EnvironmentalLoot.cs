using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentalLoot : MonoBehaviour
{
    public ItemRegistry itemRegistry;
    public DungeonManager dungeonManager;
    public bool augmentChest = false;
    public bool oreChest = false;
    public bool dropChest = false;
    void Start()
    {
        dungeonManager = FindFirstObjectByType<DungeonManager>();
        itemRegistry = FindFirstObjectByType<ItemRegistry>();
    }
    public Augment GetLootAugment()
    {
        int currentFloor = dungeonManager.getFloor();
        List<Augment> tempList = new List<Augment>();
        // Get dungeon floor and add all valid augments to tempList. Then return a random augment from tempList
        foreach (var augment in ItemRegistry.Instance.allAugments)
        {
            if (augment.dropFloor <= currentFloor) tempList.Add(augment);
        }
        if (tempList.Count == 0)
        {
            Debug.LogWarning($"No valid augments for floor {currentFloor}");
            return null;
        }
        Dictionary<AugmentRarity, List<Augment>> rarityGroups = new();
        foreach (var augment in tempList)
        {
            if (!rarityGroups.ContainsKey(augment.augmentRarity)) rarityGroups[augment.augmentRarity] = new List<Augment>();
            rarityGroups[augment.augmentRarity].Add(augment);
        }
        Dictionary<AugmentRarity, float> rarityWeights = new();
        foreach (var rarity in rarityGroups.Keys)
        {
            rarityWeights[rarity] = GetBaseAugmentDropRate(rarity);
        }

        AugmentRarity chosenRarity = GetRandomAugmentRarity(rarityWeights);
        var pool = rarityGroups[chosenRarity];
        Debug.LogError("CALLED LOOT AUGMENT");
        return pool[Random.Range(0, pool.Count)];
    }
    private float GetBaseAugmentDropRate(AugmentRarity rarity) {
        return rarity switch {
            AugmentRarity.Common => 0.8f,
            AugmentRarity.Uncommon => 0.4f,
            AugmentRarity.Rare => 0.15f,
            AugmentRarity.Epic => 0.08f,
            AugmentRarity.Legendary => 0.02f,
            AugmentRarity.Mythical => 0.005f,
            _ => 0f
        };
    }
    private AugmentRarity GetRandomAugmentRarity(Dictionary<AugmentRarity, float> weights) {
        float total = 0f;
        foreach (var w in weights.Values) total += w;
        float roll = Random.Range(0, total);
        float sum = 0f;

        foreach(var kvp in weights) {
            sum += kvp.Value;
            if (roll <= sum) return kvp.Key;
        }

        return AugmentRarity.Common;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            if (augmentChest) InventorySystem.Instance.AddAugment(GetLootAugment());
            Destroy(gameObject);
        }
    }
    
}
