using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using System.IO;
public class CraftingUIManager : MonoBehaviour
{
    public Transform inventoryParent;
    public Transform recipeSlotsParent;
    public Text previewText;
    public Button craftButton;

    public GameObject inventoryButtonPrefab;
    public GameObject recipeSlotPrefab;

    public CraftingFactory craftingFactory;
    public CraftingRecipe selectedRecipe;

    private List<Items> currentIngredients = new();
    private List<GameObject> recipeSlotUIs = new();

    private void Start()
    {
        craftButton.onClick.AddListener(CraftItem);
        PopulateInventory();
        SetupRecipeSlots();
    }
    void CraftItem() {
        if (InventorySystem.Instance == null)
{
    Debug.LogError("InventorySystem.Instance is null!");
    return;
}
        if (currentIngredients.Count != selectedRecipe.requiredSlots) return;
        Debug.LogWarning(InventorySystem.Instance);
        Equipment result = craftingFactory.GenerateFromIngredients(currentIngredients, selectedRecipe);
        Debug.Log(result);
        try {
        InventorySystem.Instance.AddEquipment(result);
        } catch(IOException e) {
            Debug.Log(e.StackTrace);
        }
        previewText.text = $"{result.itemName} crafted!";
        currentIngredients.Clear();
    }
    void PopulateInventory() {
        List<Items> allItems = new List<Items>(Resources.LoadAll<Items>("Items"));

        foreach(Items item in allItems) {
            var button = Instantiate(inventoryButtonPrefab, inventoryParent);
            button.GetComponentInChildren<Text>().text = item.itemName;

            button.GetComponent<Button>().onClick.AddListener(() => {TryAddIngredient(item);});
        }
    }
    void SetupRecipeSlots() {
        recipeSlotUIs.Clear();
        for (int i = 0; i < selectedRecipe.requiredSlots; i++) {
            var slot = Instantiate(recipeSlotPrefab, recipeSlotsParent);
            slot.GetComponentInChildren<Text>().text = $"Slot {i + 1}";
            recipeSlotUIs.Add(slot);
        }
    }
    void TryAddIngredient(Items item) {
        if (currentIngredients.Count >= selectedRecipe.requiredSlots) return;

        currentIngredients.Add(item);
        int slotIndex = currentIngredients.Count - 1;
        if (slotIndex < recipeSlotUIs.Count) {
            var text = recipeSlotUIs[slotIndex].GetComponentInChildren<Text>();
            text.text = item.itemName;
        }
        UpdatePreview();
    }
    void UpdatePreview(){
        if (currentIngredients.Count != selectedRecipe.requiredSlots) {
            previewText.text = "Select all ingredients...";
            return;
        }

        Equipment preview = craftingFactory.PreviewCraftedEquipment(currentIngredients, selectedRecipe);
        string result = $"<b>{preview.itemName}</b>\n\n";

        foreach (var mod in preview.modifiers) {
            result += $"{mod.statType}: + {mod.flatAmount}";
            if (mod.percentAmount != 0) result += $" (+{mod.percentAmount*100}% bonus)";
            result += "\n";
        }

        previewText.text = result;
    }
}
