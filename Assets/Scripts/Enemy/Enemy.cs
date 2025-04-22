using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    void Start()
    {
        DungeonManager.Instance.RegisterEnemy();

    }
    public void TakeDamage(float damage) {
        enemyStats.currentHealth = enemyStats.currentHealth - (float)damage;
        enemyStats.wasHitByPlayer = true;
        if(enemyStats.getHealth() <= 0) {
            Die();
        }
    }
    public void Die() {
        DungeonManager.Instance.OnEnemyKilled();
        List<Items> drops = enemyStats.getDrop();
        foreach(var item in drops) {
            InventorySystem.Instance.AddItem(item.ID, 1);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
