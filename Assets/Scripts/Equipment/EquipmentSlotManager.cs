using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EquipmentSlotManager : MonoBehaviour
{
    public EquipmentSlot slotType;
    public Image iconImage;
    //public TextMeshProUGUI text;
    public void SetItem(Equipment item) {
        if (item!= null) {
            try {
                iconImage.sprite = item.icon;

                iconImage.enabled = true;
            } catch (NullReferenceException e) {
                
            }
        } else {
            //iconImage.sprite = null;
            //iconImage.enabled = false;
        }
    }
}
