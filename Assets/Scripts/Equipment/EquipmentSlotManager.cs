using System;
using UnityEngine;
using UnityEngine.UI;
public class EquipmentSlotManager : MonoBehaviour
{
    public EquipmentSlot slotType;
    public Image iconImage;
    public void SetItem(Equipment item) {
        if (item!= null) {
            try {
                iconImage.sprite = ItemRegistry.Instance.GetItemById(item.itemName)?.icon;
                iconImage.enabled = true;
            } catch (NullReferenceException e) {
                Debug.LogWarning("no equipment icon image detected  " + e.StackTrace);
            }
        } else {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
    }
}
