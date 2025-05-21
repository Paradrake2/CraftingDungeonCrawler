using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AugmentManager : MonoBehaviour
{
    public AugmentManager Instance;
    public GameObject augmentSlotPrefab; // this is the slot for the equipment
    public Transform equipmentAugmentSlotHolder; // this is what will hold prefab for the augment slots
    public GameObject equipmentInventorySlot;
    public Image selectedEquipmentIcon;
    //public GameObject powderSlotPrefab;
    public GameObject equipmentSlotPrefab;
    public GameObject powderAmountSelectorPrefab;
    [Header("This holds the equipment inventory slots")]
    public Transform equipmentInventory;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI levelText;
    public Button applyButton;
    public Equipment selectedEquipment;
    public Augment selectedAugment;
    public EquipmentInventoryManager equipmentInventoryManager;
    public AugmentInventory augmentInventory;

    public PowderInventory powderInventory;
    //UI menus
    public GameObject powderApplicationUI;
    public Image powderPreviewBar;
    public TextMeshProUGUI powderPreviewText;
    public TextMeshProUGUI statsPreviewText;


    public float xp = 0;
    
    public void MainApply()
    {
        // bring up UI to confirm and show new stats of item.
        Debug.LogWarning("APPLIED AUGMENT TO EQUIPMENT(OR ATTEMPTED TO)");
        ApplyAugment.ApplyAugmentToEquipment(selectedAugment, selectedEquipment);
        GenerateAugmentSlots(selectedEquipment);
        RefreshAugmentInventory();
        selectedAugment = null;
    }
    



    // Apply powder functions
    public void PowderUIBringup() {
        powderApplicationUI.SetActive(true);
        PreviewPowderUsage(selectedEquipment);
    }
    public void PowderUIClose() {
        powderApplicationUI.SetActive(false);
    }
    public void PreviewPowderUsage(Equipment equipment) {
        int neededXp = EquipmentModifier.XpNeeded(equipment);
        int availablePowder = PowderInventory.Instance.totalPowder;

        float ratio = (float)availablePowder/neededXp;
        ratio = Mathf.Clamp01(ratio);

        powderPreviewBar.fillAmount = ratio;

        if (powderPreviewText != null) {
            powderPreviewText.text = $"{Mathf.Min(availablePowder, neededXp)} / {neededXp} XP";
        }
        ShowStatsInPowder(selectedEquipment);
    }
    public void PowderApply() {
        // bring up a UI to select how much powder to use. Converts that powder into xp which can be
        // used for the selectedEquipment. If player chooses a different equipment, the current xp is
        // converted into powder
        int neededXp = EquipmentModifier.XpNeeded(selectedEquipment);
        int availablePowder = PowderInventory.Instance.totalPowder;
        int xpToApply = Mathf.Min(neededXp, availablePowder);
        PowderInventory.Instance.RemovePowder(xpToApply);
        EquipmentModifier.EnhanceWithPowder(xpToApply, selectedEquipment);
        PreviewPowderUsage(selectedEquipment);
    }
    void ShowStatsInPowder(Equipment selectedEquipment) {
        foreach(var mod in selectedEquipment.modifiers) {
            if (mod.flatAmount >0f || mod.percentAmount > 0f ) {
                statsPreviewText.text = $"{mod.statType}: Flat {mod.flatAmount} Percent {mod.percentAmount} + 0.01 \n";
            }
        }
    }



    public void RefreshAugmentInventory()
    {
        augmentInventory.PopulateInventory((Augment augment) =>
        {
            //selectedEquipment.EquipAugment(selectedAugment);
            //SelectAugment(augment);
            if (selectedEquipment == null) return;

            if (selectedEquipment.appliedAugments.Count < selectedEquipment.augmentSlotNumber)
            {
                ApplyAugment.ApplyAugmentToEquipment(augment, selectedEquipment);
                GenerateAugmentSlots(selectedEquipment);
                RefreshAugmentInventory();
            }
            else
            {
                Debug.LogWarning("No available augment slots.");
            }
        });
    }

    public void SelectEquipment(Equipment item)
    {
        selectedEquipment = item;
        if (selectedEquipment != null)
        {
            selectedEquipmentIcon.sprite = item.icon;
            selectedEquipmentIcon.enabled = true;
            GenerateAugmentSlots(selectedEquipment);
        }
        //MainApply();
        RefreshAugmentInventory();

    }

    public void SelectAugment(Augment augment) {
        Debug.LogWarning($"SELECTED {augment.augmentName}");
        selectedAugment = augment;
        if (selectedAugment != null) {
            //selectedAugment.icon = augment.icon;
            //Image augIm = augmentSlotPrefab.GetComponent<Image>();
            //augIm.sprite = augment.icon;
        }
        GenerateAugmentSlots(selectedEquipment);
    }
    // for equipmentAugmentSlotPrefab, the number of times it is generated depends on equipment augment slots
    public void GenerateAugmentSlots(Equipment selectedEquipment) {
        // Clear old slots first
        foreach (Transform child in equipmentAugmentSlotHolder) Destroy(child.gameObject);
        int augmentSlotCount = selectedEquipment.augmentSlotNumber;
        for (int i = 0; i < augmentSlotCount; i++)
        {
            // instantiate the augment slots
            GameObject slot = Instantiate(augmentSlotPrefab, equipmentAugmentSlotHolder);
            AugmentSlotUI augmentSlotUI = slot.GetComponent<AugmentSlotUI>();
            augmentSlotUI.Setup(this, selectedEquipment, i);
        }
    }

    // need to have a panel for the augment selection
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        powderInventory = FindFirstObjectByType<PowderInventory>();
        equipmentInventoryManager.PopulateInventory((Equipment item) => {
            SelectEquipment(item);
        });
        augmentInventory.PopulateInventory((Augment augment) => {
            SelectAugment(augment);
        });
    }
    public void ShowEquipmentStats() {

    }
    public void loadEquipmentScene() {
        SceneManager.LoadScene("EquipmentMenu");
    }
}
