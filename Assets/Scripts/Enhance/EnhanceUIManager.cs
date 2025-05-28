using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnhanceUIManager : MonoBehaviour
{
    public static EnhanceUIManager Instance;
    public Transform inventoryParent;
    public GameObject enhanceInventoryButton;
    public Transform enhanceItemParent;
    public GameObject enhanceItemSlot;
    public TextMeshProUGUI succcessChance;
    public TextMeshProUGUI hoverText;
    public EnhanceItem enhanceItem;
    public Items selectedItem;
    public void PopulateEnhanceInventory()
    {
        ClearChildren(inventoryParent);

        foreach (var stack in InventorySystem.Instance.itemStacks)
        {
            Items itemData = ItemRegistry.Instance.GetItemById(stack.itemId);
            if (itemData == null) continue;
            GameObject button = Instantiate(enhanceInventoryButton, inventoryParent);
            SetupEnhanceInventoryButtons(button, itemData, stack.quantity);
            
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectEnhanceItem(itemData);
            });
        }
    }
    public void LoadCrafting()
    {
        SceneManager.LoadScene("Menu");
    }
    void SetupEnhanceInventoryButtons(GameObject button, Items item, int quantity)
    {
        Image icon = button.GetComponentInChildren<Image>();
        icon.sprite = item.icon;
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "x" + quantity;
        var trigger = button.AddComponent<UnityEngine.EventSystems.EventTrigger>();

        // Pointer enter
        var enterEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
        enterEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) => { HoverTextShow(item); });
        trigger.triggers.Add(enterEntry);

        var exitEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
        exitEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { HideHoverText(); });
        trigger.triggers.Add(exitEntry);
    }
    public void SelectEnhanceItem(Items item)
    {
        selectedItem = item;
        DisplaySelectedItem(item);
        UpdateSuccessChance(selectedItem);
    }
    void HoverTextShow(Items item)
    {
        hoverText.text = item.itemName;
        hoverText.gameObject.SetActive(true);
    }
    void HideHoverText()
    {
        hoverText.text = "";
        hoverText.gameObject.SetActive(false);
    }
    public void UpdateSuccessChance(Items item)
    {
        succcessChance.text = (enhanceItem.GetSuccessChance(selectedItem)*100).ToString() + "%";
    }
    bool HasEnoughItems()
    {
        int quantity = InventorySystem.Instance.GetQuantity(selectedItem.itemName);
        if (quantity > 0) return true;
        return false;
    }
    void DisplaySelectedItem(Items item)
    {
        Image icon = IconSlot();
        icon.sprite = item.icon;
    }
    public void EnhanceItem()
    {
        if (HasEnoughItems())
        {
            enhanceItem.EnhanceMaterial(selectedItem);
            PopulateEnhanceInventory();
            CheckIfEnough();
        }
    }
    void CheckIfEnough()
    {
        int quantity = InventorySystem.Instance.GetQuantity(selectedItem.itemName);
        if (quantity == 0)
        {
            selectedItem = null;
            Image icon = IconSlot();
            icon.sprite = null;
        }
    }
    Image IconSlot()
    {
        return enhanceItemSlot.GetComponentInChildren<Image>();
    }
    void ClearChildren(Transform parent = null)
    {
        foreach (Transform child in parent) Destroy(child.gameObject);
    }
    void Start()
    {
        PopulateEnhanceInventory();
    }

    void Update()
    {
        
    }
}
