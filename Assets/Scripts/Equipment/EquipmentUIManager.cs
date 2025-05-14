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
        //PopulateInventory(equipmentButtonPrefab, inventoryPanel);
    }
    /*
    public void PopulateInventory(GameObject equipmentButtonPrefab, Transform inventoryPanel) {
        foreach (var item in InventorySystem.Instance.ownedGear) {
            GameObject btn = Instantiate(equipmentButtonPrefab, inventoryPanel);
            // below is placeholder for when i have sprite generation and shit, idk its just a placeholder dont @ me
            btn.GetComponentInChildren<Image>().sprite = GetSpriteForEquipment(item);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;

            btn.GetComponent<Button>().onClick.AddListener(() => EquipToSlot(item));
        }
    }
    
    Sprite GetSpriteForEquipment(Equipment equipment) {
        return equipment.icon;
    }
    PlayerStats.Instance.EquipItem(item);
                EquipmentUIManager.Instance?.RefreshSlots(); // Trigger slot update

    */
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
