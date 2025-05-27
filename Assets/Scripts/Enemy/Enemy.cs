using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    public PlayerStats playerStats;
    [SerializeField] FloatingHealthBarSlider healthBar;
    
    void Start()
    {
        DungeonManager.Instance.RegisterEnemy();
        healthBar = GetComponentInChildren<FloatingHealthBarSlider>();
        if (PlayerStats.Instance != null)
        {
            playerStats = PlayerStats.Instance;
        }
        else
        {
            Debug.LogError("PlayerStats instance not found");
        }
    }
    public void TakeDamage(float damage) {
        enemyStats.currentHealth = enemyStats.currentHealth - (float)damage;
        enemyStats.wasHitByPlayer = true;
        healthBar.ShowHealthBar();
        healthBar.UpdateHealthBar(enemyStats.currentHealth, enemyStats.maxHealth);
        if (enemyStats.getHealth() <= 0)
        {
            Die();
        }
    }
    public void Die() {
        DungeonManager.Instance.OnEnemyKilled();
        List<Items> drops = enemyStats.getDrop();
        foreach(var item in drops) {
            InventorySystem.Instance.AddItem(item.ID, 1);
        }
        playerStats.GainXP(enemyStats.getXP());
        Destroy(gameObject);
    }
    void Update()
    {
        
    }
}
