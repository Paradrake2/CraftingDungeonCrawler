using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class AugmentSlotUI : MonoBehaviour
{
    public UnityEngine.UI.Image icon;
    public Button button;
    public Sprite emptySlotIcon;
    public AugmentManager augmentManager;
    private int slotIndex;
    private Equipment currentEquipment;

    // Show the augments for the selected equipment
    public void Setup(AugmentManager manager, Equipment equipment, int index) {
        currentEquipment = equipment;
        slotIndex = index;
        augmentManager = manager;
        Debug.LogWarning("SETUP AUGMENTSLOT CALLED");
        /*
        if (index < equipment.appliedAugments.Count)
        {
            var augment = equipment.appliedAugments[index];
            icon.sprite = augment.icon;
        }
        else
        {
            icon.sprite = emptySlotIcon;
        }
        */
        UpdateSlotUI();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickSlot);
    }
    void OnClickSlot() {
        if (slotIndex < currentEquipment.appliedAugments.Count)
        {
            currentEquipment.RemoveAugment(currentEquipment.appliedAugments[slotIndex]);
            augmentManager.GenerateAugmentSlots(currentEquipment);
            //augmentManager.MainApply();
            augmentManager.RefreshAugmentInventory();
            UpdateSlotUI();
        }
        /*
        else if (augmentManager.selectedAugment != null)
        {
            if (currentEquipment.TryAddAugment(currentEquipment))
            { // was TryAddAugment(currentEquipment)
                ApplyAugment.ApplyAugmentToEquipment(augmentManager.selectedAugment, currentEquipment);
                augmentManager.MainApply();
                augmentManager.selectedAugment = null;
                UpdateSlotUI();
            }
        }
        */
    }
    public void UpdateSlotUI()
    {
        if (slotIndex < currentEquipment.appliedAugments.Count)
        {
            var augment = currentEquipment.appliedAugments[slotIndex];
            icon.sprite = augment.icon;
            // text.text = augment.augmentName;
        }
        else
        {
            // icon.sprite = empty slot icon
            // text.text = empty
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
