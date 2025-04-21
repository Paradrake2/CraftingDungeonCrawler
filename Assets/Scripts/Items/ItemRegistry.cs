using System.Collections.Generic;
using UnityEngine;

public class ItemRegistry : MonoBehaviour
{
    public static ItemRegistry Instance;

    private Dictionary<string, Items> itemLookup = new();

    public List<Items> allItems = new(); // Optional for debugging

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAllItems();
    }

    public void LoadAllItems()
    {
        Items[] items = Resources.LoadAll<Items>("Items");
        allItems = new List<Items>(items);

        foreach (var item in items)
        {
            if (!string.IsNullOrEmpty(item.ID))
            {
                itemLookup[item.ID] = item;
            }
            else
            {
                Debug.LogWarning($"Item '{item.name}' has no ID set.");
            }
        }

        Debug.Log($"[ItemRegistry] Loaded {itemLookup.Count} items.");
    }

    public Items GetItemById(string id)
    {
        if (itemLookup.TryGetValue(id, out var item))
            return item;

        Debug.LogWarning($"Item ID '{id}' not found.");
        return null;
    }
}
