using System.Linq;
using UnityEngine;

public class RecipeInputSlot : MonoBehaviour
{
    public string requiredTag;
    public int quantityRequired;
    public Items assignedItem;
    
    public void SetSlot(string tag, int qty) {
        requiredTag = tag;
        quantityRequired = qty;
    }

    public bool IsValidItem(Items item) {
        return item.tags.Contains(requiredTag);
    }
}
