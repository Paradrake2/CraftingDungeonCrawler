using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AugmentInventory : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public GameObject augmentInventorySlot;
    public Transform augmentInventory;
    public void PopulateInventory(Action<Augment> onClickAction) {
        foreach (Transform child in augmentInventory) {
            Destroy(child.gameObject); // clear inventory
        }
        foreach (var augment in InventorySystem.Instance.ownedAugments) {
            GameObject btn = Instantiate(augmentInventorySlot, augmentInventory);
            var icon = btn.GetComponentInChildren<Image>();
            var text = btn.GetComponentInChildren<TextMeshProUGUI>();

            if (icon != null) icon.sprite = augment.icon;
            if (text != null) text.text = augment.augmentName;

            btn.GetComponent<Button>().onClick.AddListener(() => onClickAction?.Invoke(augment));
        }
    }

    void Start()
    {
        inventorySystem = FindFirstObjectByType<InventorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
