using UnityEngine;

public static class ApplyAugment
{
    
    public static void ApplyAugmentToEquipment(Augment augment, Equipment equipment) {
        if (!equipment.TryAddAugment(equipment)) return;
        if (augment != null) equipment.appliedAugments.Add(augment);
        else return;
        InventorySystem.Instance.RemoveAugment(augment);
        equipment.allowedAugments--;
        foreach(var mod in augment.modifiers) {
            equipment.modifiers.Add(new StatModifier {
                statType = mod.statType,
                flatAmount = mod.flatAmount,
                percentAmount = mod.percentAmount
            });
        }
    }
    public static void RemoveAugment(int index, Equipment equipment) {
        if (index >= 0 && index < equipment.appliedAugments.Count)
        {
            InventorySystem.Instance.AddAugment(equipment.appliedAugments[index]);
            equipment.appliedAugments.RemoveAt(index);
            equipment.allowedAugments++;
        }
    }
}
