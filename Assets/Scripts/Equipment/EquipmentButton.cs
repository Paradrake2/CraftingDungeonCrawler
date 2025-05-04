using UnityEngine;
using UnityEngine.UI;

public class EquipmentButton : MonoBehaviour
{
    public bool isAccessory = false;
    public EquipmentSlot slotType;
    public int accessoryIndex;
    private Button button;
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    
    private void OnClick() {
        if (!isAccessory) {
            Equipment equippedItem = PlayerStats.Instance.GetEquippedItem(slotType);

            if (equippedItem != null) {
                Debug.Log($"Clicked {slotType} slot: Equipped {equippedItem.itemName}");
                // TODO: open equipment details/unequip/replace
            } else {
                Debug.Log($"Clicked {slotType} slot: Empty");
            }
        }
        else {
            Equipment accessoryItem = PlayerStats.Instance.GetAccessoryAt(accessoryIndex);
            if (accessoryItem != null)
            {
                Debug.Log($"Clicked Accessory Slot {accessoryIndex}: Equipped {accessoryItem.itemName}");
                // TODO: Open accessory details / unequip / replace
            }
            else
            {
                Debug.Log($"Clicked Accessory Slot {accessoryIndex}: Empty");
            }
        }
    }
}
