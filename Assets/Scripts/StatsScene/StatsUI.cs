using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsUI : MonoBehaviour
{
    private PlayerStats playerStats;
    public TextMeshProUGUI health;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI xp;
    public TextMeshProUGUI mana;
    public TextMeshProUGUI magicDamage;
    public TextMeshProUGUI knockback;
    public void loadEquipment() {
        SceneManager.LoadScene("EquipmentMenu");
    }
    void Start()
    {
        UpdateStatsDisplay();
    }

    void UpdateStatsDisplay() {
        playerStats = PlayerStats.Instance;
        health.text = "Max health: " + playerStats.CurrentMaxHealth;
        damage.text = "Damage: " + playerStats.CurrentDamage;
        defense.text = "Defense: " + playerStats.CurrentDefense;
        xp.text = "XP: " + playerStats.XP;
        mana.text = "Max Mana: " + playerStats.CurrentMaxMana;
        magicDamage.text = "Magic Damage: " + playerStats.CurrentMagicDamage;
        knockback.text = "Knockback " + playerStats.CurrentKnockback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
