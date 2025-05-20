using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUIManager : MonoBehaviour
{
    public Transform inventoryPanel;
    public static EquipmentUIManager Instance;
    public GameObject equipmentButtonPrefab;
    public List<EquipmentSlotManager> slotManagers;
    public EquipmentInventoryManager equipmentInventoryManager;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        RefreshSlots();
        equipmentInventoryManager.PopulateInventory((Equipment item) => {
            PlayerStats.Instance.EquipItem(item);
            EquipmentUIManager.Instance.RefreshSlots();
        });
    }
    public void RefreshSlots() {
        foreach (var slot in slotManagers) {
            if (slot.isAccessory) {
                Equipment accessory = PlayerStats.Instance.GetAccessoryAt(slot.accessoryIndex);
                Image slotImage = slot.GetComponentInChildren<Image>();
                slot.SetItem(accessory);
                slotImage.sprite = accessory.icon;
            } else {
                if (PlayerStats.Instance.equippedItems.TryGetValue(slot.slotType, out var equippedItem)) {
                    slot.SetItem(equippedItem);
                    Image slotImage = slot.GetComponentInChildren<Image>();
                    slotImage.sprite = equippedItem.icon;
                }
                else {
                    slot.SetItem(null);
                }
            }
        }
    }
    void EquipToSlot(Equipment item) {
        PlayerStats.Instance.EquipItem(item);
        RefreshSlots();
        // Update visuals
    }
}
