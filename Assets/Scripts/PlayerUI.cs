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
    
    void Start()
    {
        playerStats = PlayerStats.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats != null && healthText != null) {
            healthText.text = $"Health: {Mathf.RoundToInt(playerStats.CurrentHealth)} / {Mathf.RoundToInt(playerStats.CurrentMaxHealth)}";
            floorNum.text = $"Floor: {dungeonManager.floor}";
            enemiesKilled.text = $"Enemies killed: {dungeonManager.enemiesKilled}";
        
        
        }
    }
}
