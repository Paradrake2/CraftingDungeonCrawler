using UnityEngine;

public class ApplyAugment : MonoBehaviour
{
    public void ApplyAugmentToEquipment(Augment augment, Equipment equipment) {
        if (equipment.allowedAugments <= 0) return;
        equipment.appliedAugments.Add(augment);
        equipment.allowedAugments--;
        foreach(var mod in augment.modifiers) {
            equipment.modifiers.Add(new StatModifier {
                statType = mod.statType,
                flatAmount = mod.flatAmount,
                percentAmount = mod.percentAmount
            });
        }
    }
}
