using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public DungeonManager dungeonManager;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI floorNum;
    public TextMeshProUGUI enemiesKilled;
    public TextMeshProUGUI manaText;

    void Start()
    {
        playerStats = PlayerStats.Instance;
        dungeonManager = GameObject.FindFirstObjectByType<DungeonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats != null && healthText != null && manaText != null) {
            healthText.text = $"Health: {Mathf.RoundToInt(playerStats.CurrentHealth)} / {Mathf.RoundToInt(playerStats.CurrentMaxHealth)}";
            floorNum.text = $"Floor: {dungeonManager.floor}";
            enemiesKilled.text = $"Enemies killed: {dungeonManager.enemiesKilled}";
            manaText.text = $"{playerStats.CurrentMana} / {playerStats.CurrentMaxMana}";
        
        }
    }
}
