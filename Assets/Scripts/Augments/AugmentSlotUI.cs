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
    public void Setup(Equipment equipment, int index) {
        currentEquipment = equipment;
        slotIndex = index;

        if (index < equipment.appliedAugments.Count) {
            var augment = equipment.appliedAugments[index];
            icon.sprite = augment.icon;
        } else {
            icon.sprite = emptySlotIcon;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickSlot);
    }
    void OnClickSlot() {
        if (slotIndex < currentEquipment.appliedAugments.Count) {
            currentEquipment.RemoveAugment(currentEquipment.appliedAugments[slotIndex]);
        } else if (augmentManager.selectedAugment != null) {
            if (currentEquipment.TryAddAugment(currentEquipment)) {
                ApplyAugment.ApplyAugmentToEquipment(augmentManager.selectedAugment, currentEquipment);
                augmentManager.selectedAugment = null;
            }
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
