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
    public TextMeshProUGUI previewText;
    public Button craftButton;

    public GameObject inventoryButtonPrefab;
    public GameObject recipeSlotPrefab;

    public CraftingFactory craftingFactory;
    public CraftingRecipe selectedRecipe;

    private List<Items> currentIngredients = new();
    private List<GameObject> recipeSlotUIs = new();
    public List<CraftingRecipe> allRecipes;
    public Transform recipeButtonParent;
    public GameObject recipeButtonPrefab;
    public static CraftingUIManager Instance;
    private void Start()
    {
        craftButton.onClick.AddListener(CraftItem);
        PopulateInventory();
        SetupRecipeSlots();
        GenerateRecipeButtons();
        Instance = this;
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
    public void RefreshInventoryUI() {
        PopulateInventory();
    }
    void PopulateInventory() {
        /*
        // this is for testing
        List<Items> allItems = new List<Items>(Resources.LoadAll<Items>("Items"));

        foreach(Items item in allItems) {
            var button = Instantiate(inventoryButtonPrefab, inventoryParent);
            var iconImage = button.transform.Find("Image").GetComponent<Image>();
            iconImage.sprite = item.icon;
            button.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
            button.GetComponent<Button>().onClick.AddListener(() => {TryAddIngredient(item);});
        }
        */

        // this is for actual use
        foreach(Transform child in inventoryParent) Destroy(child.gameObject); // clear old buttons
        foreach (var stack in InventorySystem.Instance.itemStacks) {
            Items itemData = ItemRegistry.Instance.GetItemById(stack.itemId);
            if (itemData == null) continue;
            GameObject button = Instantiate(inventoryButtonPrefab, inventoryParent);

            Image icon = button.GetComponentInChildren<Image>();
            if (icon != null) icon.sprite = itemData.icon;

            // Set up text
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null) text.text = $"{itemData.itemName} x {stack.quantity}";

            button.GetComponent<Button>().onClick.AddListener(() => {
                TryAddIngredient(itemData);
            });
        }
    }
    void SetupRecipeSlots() {
        recipeSlotUIs.Clear();
        for (int i = 0; i < selectedRecipe.requiredSlots; i++) {
            var slot = Instantiate(recipeSlotPrefab, recipeSlotsParent);
//            slot.GetComponentInChildren<Text>().text = $"Slot {i + 1}";
            recipeSlotUIs.Add(slot);
        }
    }
    void GenerateRecipeButtons() {
        foreach (var recipe in allRecipes) {
            GameObject buttonObject = Instantiate(recipeButtonPrefab, recipeButtonParent);
            buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = recipe.getRecipeName();
            buttonObject.GetComponentInChildren<Image>().sprite = recipe.getIcon();

            var button = buttonObject.GetComponent<Button>();
            if (button != null) {
                button.onClick.AddListener(() => SelectRecipe(recipe));
            }

        }
    }
    public void SelectRecipe(CraftingRecipe recipe) {
        selectedRecipe = recipe;
        currentIngredients.Clear();
        SetupRecipeSlots();
        UpdatePreview();
    }
    void TryAddIngredient(Items item) {
        if (currentIngredients.Count >= selectedRecipe.requiredSlots) return;

        currentIngredients.Add(item);
        int slotIndex = currentIngredients.Count - 1;
        if (slotIndex < recipeSlotUIs.Count) {
            var text = recipeSlotUIs[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
            //text.text = item.itemName;
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
