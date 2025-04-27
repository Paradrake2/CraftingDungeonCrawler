using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
public class RefiningUIManager : MonoBehaviour
{
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;
    public TextMeshProUGUI ingredientText;
    public Button refineButton;
    public Image itemIcon;
    private Items selectedRefinedItem;
    public static RefiningUIManager Instance;
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
    public void RefreshRefineUI() {
        foreach (Transform child in recipeListParent) Destroy(child.gameObject);
        GenerateRecipeList();
        CraftingUIManager.Instance.selectedRecipe = null;
        CraftingUIManager.Instance.SelectRecipe(null);
    }
    void GenerateRecipeList() {
        Items[] allItems = Resources.LoadAll<Items>("");
        foreach(Items item in allItems) {
            if (item.isCraftable && item.isCraftingMaterial) {
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
                if (icon != null) icon.sprite = item.icon;
                btn.GetComponent<Button>().onClick.AddListener(() =>{
                    selectedRefinedItem = item;
                    UpdateIngredientPreview();
                });
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
            var mat = selectedRefinedItem.requiredMaterials[i];
            var needed = selectedRefinedItem.requiredQuantities[i];
            display += $"{mat.itemName} x{needed}\n";
        }

        ingredientText.text = display;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
