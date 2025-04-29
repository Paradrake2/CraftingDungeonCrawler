using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentButtonManager : MonoBehaviour
{
    public void loadCraftingScene() {
        SceneManager.LoadScene("Menu");
    }
    
    public void loadDungeon() {
        PlayerStats.Instance.CurrentHealth = PlayerStats.Instance.CurrentMaxHealth;
        SceneManager.LoadScene("Dungeon");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
