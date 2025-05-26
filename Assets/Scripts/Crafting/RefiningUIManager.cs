using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System;
public class RefiningUIManager : MonoBehaviour
{
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;
    public GameObject refinePowderPrefab;
    public TextMeshProUGUI ingredientText;
    public TextMeshProUGUI statstext;
    public Button refineButton;
    public Image itemIcon;
    public static RefiningUIManager Instance;

    [Header("New refine stuff")]
    public RefineRecipe selectedRecipe;
    public List<Items> currentIngredients = new();
    private List<GameObject> recipeSlotUIs = new();
    private Dictionary<string, int> assignedCounts = new();

    public Transform recipeButtonParent;
    public TextMeshProUGUI previewText;
    public CraftingUIManager craftingUIManager;
    public InventorySystem inventorySystem;

    public List<Items> lastUsedIngredients = new();
    void Start()
    {
        refineButton.onClick.AddListener(RefineItem);
        inventorySystem = FindFirstObjectByType<InventorySystem>();
        GenerateRecipeButtons();
    }
    void RefineItem()
    {
        if (currentIngredients.Count != selectedRecipe.requirements.Sum(req => req.quantityRequired)) return;
        bool canRefineAgain = true;
        foreach (var item in currentIngredients)
        {
            if (!InventorySystem.Instance.HasItem(item.ID, 1))
            {
                canRefineAgain = false;
                break;
            }
        }

        foreach (var item in currentIngredients)
        {
            InventorySystem.Instance.RemoveItem(item.ID, 1);
        }
        lastUsedIngredients = new List<Items>(currentIngredients);
        inventorySystem.AddItem(selectedRecipe.outputItem.ID, selectedRecipe.outputQuantity);
        inventorySystem.discoveredRefinedItems.Add(selectedRecipe.outputItem.ID);
        previewText.text = $"{selectedRecipe.outputItem.itemName} created!";
        currentIngredients.Clear();
        assignedCounts = new Dictionary<string, int>();
        craftingUIManager.PopulateInventory();
        SetupRecipeSlots();
        if (canRefineAgain)
        {
            TryAutoAssignLastUsedIngredients();
        }
        Debug.LogWarning(canRefineAgain.ToString());
    }
    void TryAutoAssignLastUsedIngredients()
    {
        foreach (var item in lastUsedIngredients)
        {
            if (inventorySystem.HasItem(item.ID, 1))
            {
                TryAddIngredient(item);
            }
        }
    }
    public void GenerateRecipeButtons()
    {
        HoverNameUI.Instance.Hide();
        RefineRecipe[] allRecipes = Resources.LoadAll<RefineRecipe>("RefineRecipe");
        foreach (Transform child in recipeListParent) Destroy(child.gameObject);
        foreach (var recipe in allRecipes)
        {
            bool canCraftNow = true;
            try
            {
                foreach (var req in recipe.requirements)
                {
                    if (!inventorySystem.HasItemsWithTag(req.requiredTag, req.quantityRequired))
                    {
                        canCraftNow = false;
                        break;
                    }
                }

                bool hasCraftedBefore = inventorySystem.discoveredRefinedItems.Contains(recipe.outputItem.ID);
                if (canCraftNow || hasCraftedBefore)
                {
                    GameObject btn = Instantiate(recipeButtonPrefab, recipeListParent);
                    var eventTrigger = btn.AddComponent<EventTrigger>();

                    btn.GetComponentInChildren<Image>().sprite = recipe.icon;

                    // Triggers
                    var enter = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerEnter
                    };
                    enter.callback.AddListener((eventData) => { HoverNameUI.Instance.Show(recipe.outputItem.itemName); });
                    eventTrigger.triggers.Add(enter);

                    var exit = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerExit
                    };
                    exit.callback.AddListener((eventData) => { HoverNameUI.Instance.Hide(); });
                    eventTrigger.triggers.Add(exit);

                    btn.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        SelectRecipe(recipe);
                    });

                }
            }
            catch (NullReferenceException e)
            {
                //Debug.LogError(e.StackTrace);
            }


        }
        craftingUIManager.PopulateInventory();
        craftingUIManager.recipeInfoText.text = ""; // clear recipe info text
    }

    void SelectRecipe(RefineRecipe recipe)
    {
        selectedRecipe = recipe;
        currentIngredients.Clear();
        assignedCounts = new Dictionary<string, int>();
        SetupRecipeSlots();

        CraftingUIManager.Instance.recipeInfoText.text = $"<b>{recipe.recipeName}</b>\n";
        foreach (var req in recipe.requirements)
        {
            CraftingUIManager.Instance.recipeInfoText.text += $"- {req.requiredTag}: x{req.quantityRequired}\n";
        }
    }

    void SetupRecipeSlots()
    {
        HoverNameUI.Instance.Hide();
        foreach (Transform child in recipeButtonParent)
            Destroy(child.gameObject);

        recipeSlotUIs.Clear();
        if (selectedRecipe == null) return;

        foreach (var req in selectedRecipe.requirements)
        {
            for (int i = 0; i < req.quantityRequired; i++)
            {
                GameObject slot = Instantiate(recipeButtonPrefab, recipeButtonParent);
                var text = slot.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null) text.text = $"Needs: {req.requiredTag}";
                recipeSlotUIs.Add(slot);
            }
        }
        Debug.LogWarning(selectedRecipe.GetTags());
        craftingUIManager.DisplayShader(selectedRecipe.GetTags());
        craftingUIManager.PopulateInventory(selectedRecipe.GetTags());
    }

    public void TryAddIngredient(Items item)
    {
        foreach (var tag in item.tags)
        {
            var req = selectedRecipe.requirements.FirstOrDefault(r => r.requiredTag == tag);
            if (req != null)
            {
                int alreadyAssigned = assignedCounts.ContainsKey(tag) ? assignedCounts[tag] : 0;
                if (alreadyAssigned < req.quantityRequired)
                {
                    int alreadyUsed = currentIngredients.Count(i => i.itemName == item.itemName);
                    int available = InventorySystem.Instance.GetQuantity(item.itemName);
                    if (alreadyUsed >= available)
                    {
                        Debug.Log(alreadyUsed);
                        Debug.Log(available);
                        Debug.Log($"Cannot add more {item.itemName}!");
                        continue;
                    }

                    currentIngredients.Add(item);
                    if (!assignedCounts.ContainsKey(tag)) assignedCounts[tag] = 0;
                    assignedCounts[tag]++;

                    int slotIndex = currentIngredients.Count - 1;
                    if (slotIndex < recipeSlotUIs.Count)
                    {
                        var text = recipeSlotUIs[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
                        var icon = recipeSlotUIs[slotIndex].GetComponentInChildren<Image>();
                        if (text != null) text.text = item.itemName;
                        if (icon != null) icon.sprite = item.icon;
                    }

                    return;
                }
            }
        }
    }


    /*
    void UpdateIngredientPreview() {
        if (selectedRecipe == null) {
            ingredientText.text = "Select a refined item.";
            itemIcon.sprite = null;
            return;
        }
        itemIcon.sprite = selectedRecipe.icon;

        string display = "<b>Requires:</b>\n";
        foreach (var req in selectedRecipe.requirements)
        {
            display += $"- {req.requiredTag} x{req.quantityRequired}";
        }
        string statDisplay = "Stats: " + TryAddStat(selectedRefinedItem);

        statstext.text = statDisplay;
        ingredientText.text = display;
    }
    string TryAddStat( Items refinedItem) {
        string statDisplay = "";
        if(refinedItem.flatMaxHP != 0) statDisplay += "flat HP: " + refinedItem.flatMaxHP + ", ";
        if(refinedItem.HPMult != 0) statDisplay += "HP mult: " + refinedItem.HPMult + ", ";
        if(refinedItem.flatDamage != 0) statDisplay += "flat damage: " + refinedItem.flatDamage + ", ";
        if(refinedItem.damageMult != 0) statDisplay += "damage mult: " + refinedItem.damageMult + ", ";
        if(refinedItem.flatDefense != 0) statDisplay += "flat defense: " + refinedItem.flatDefense + ", ";
        if(refinedItem.defenseMult != 0) statDisplay += "defense mult: " + refinedItem.defenseMult + ", ";
        if(refinedItem.dropRateIncrease != 0) statDisplay += "drop rate increase: " + refinedItem.dropRateIncrease + ", ";
        if(refinedItem.goldGainIncrease != 0) statDisplay += "gold gain increase: " + refinedItem.goldGainIncrease + ", ";
        return statDisplay;
        
    }
    void Start()
    {
        
        GenerateRecipeList();
        refineButton.onClick.AddListener(() => {
            Debug.Log(selectedRefinedItem);
            if (selectedRefinedItem != null) {
                RefiningManager.Instance.Refine(selectedRefinedItem);
                UpdateIngredientPreview();
            }
        });
        

    }
    public void RefreshRefineUI()
    {
        foreach (Transform child in recipeListParent) Destroy(child.gameObject);
        GenerateRecipeList();
        CraftingUIManager.Instance.selectedRecipe = null;
        CraftingUIManager.Instance.SelectRecipe(null);
    }
    bool CanRefine() {
        return false;
    }
    void GenerateRecipeList() {
        Items[] allItems = Resources.LoadAll<Items>("");
        foreach(Items item in allItems) {
            if (item.isCraftable && item.isCraftingMaterial) {
                bool canCraftNow = true;
                for (int i = 0; i < item.requiredMaterials.Count; i++) {
                    var reqMat = item.requiredMaterials[i];
                    int needed = item.requiredQuantities[i];
                    if (!InventorySystem.Instance.HasItem(reqMat.ID, needed)) {
                        canCraftNow = false;
                        break;
                    }
                }
                bool hasCraftedBefore = InventorySystem.Instance.discoveredRefinedItems.Contains(item.ID);
                if (canCraftNow || hasCraftedBefore) {
                    var btn = Instantiate(recipeButtonPrefab, recipeListParent);
                    var eventTrigger = btn.AddComponent<EventTrigger>();
                    
                    // on pointer enter
                    EventTrigger.Entry enter = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
                    enter.callback.AddListener((eventData) => {HoverNameUI.Instance.Show(item.itemName);});
                    eventTrigger.triggers.Add(enter);

                    // on pointer exit
                    EventTrigger.Entry exit = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
                    exit.callback.AddListener((eventData) => {HoverNameUI.Instance.Hide();});
                    eventTrigger.triggers.Add(exit);

                    btn.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
                    var icon = btn.transform.Find("Icon")?.GetComponent<Image>();
                    if (icon != null) {
                        icon.sprite = item.icon;
                        icon.preserveAspect = true;
                    } 

                    btn.GetComponent<Button>().onClick.AddListener(() =>{
                        selectedRefinedItem = item;
                        UpdateIngredientPreview();
                    });
                }
            }
        }
    }
    void UpdateIngredientPreview() {
        if (selectedRefinedItem == null) {
            ingredientText.text = "Select a refined item.";
            itemIcon.sprite = null;
            return;
        }
        itemIcon.sprite = selectedRefinedItem.icon;

        string display = "<b>Requires:</b>\n";
        for (int i = 0; i< selectedRefinedItem.requiredMaterials.Count; i++) {
            if (i >0) display += $", ";
            var mat = selectedRefinedItem.requiredMaterials[i];
            var needed = selectedRefinedItem.requiredQuantities[i];
            display += $"{mat.itemName} x{needed}";
        }
        string statDisplay = "Stats: " + TryAddStat(selectedRefinedItem);

        statstext.text = statDisplay;
        ingredientText.text = display;
    }
    string TryAddStat( Items refinedItem) {
        string statDisplay = "";
        if(refinedItem.flatMaxHP != 0) statDisplay += "flat HP: " + refinedItem.flatMaxHP + ", ";
        if(refinedItem.HPMult != 0) statDisplay += "HP mult: " + refinedItem.HPMult + ", ";
        if(refinedItem.flatDamage != 0) statDisplay += "flat damage: " + refinedItem.flatDamage + ", ";
        if(refinedItem.damageMult != 0) statDisplay += "damage mult: " + refinedItem.damageMult + ", ";
        if(refinedItem.flatDefense != 0) statDisplay += "flat defense: " + refinedItem.flatDefense + ", ";
        if(refinedItem.defenseMult != 0) statDisplay += "defense mult: " + refinedItem.defenseMult + ", ";
        if(refinedItem.dropRateIncrease != 0) statDisplay += "drop rate increase: " + refinedItem.dropRateIncrease + ", ";
        if(refinedItem.goldGainIncrease != 0) statDisplay += "gold gain increase: " + refinedItem.goldGainIncrease + ", ";
        return statDisplay;
        
    }
    void Update()
    {
        
    }
    */
}
