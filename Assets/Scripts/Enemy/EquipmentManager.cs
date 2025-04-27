using UnityEngine;
using System.Collections.Generic;
public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    public Dictionary<EquipmentSlot, Equipment> equippedItems = new();
    public Equipment[] accessories = new Equipment[5];
    
    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void EquipItem(Equipment newItem) {
        equippedItems[newItem.slot] = newItem;
    }

    private bool MeetsRequirements(Equipment equipment) {
        // placeholder for now, eventually can check stuff like level
        return true;
    }

    public void UpdatePlayerStats() {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
