using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    void Start()
    {
        DungeonManager.Instance.RegisterEnemy();

    }
    public void Die() {
        DungeonManager.Instance.OnEnemyKilled();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyStats.getHealth() <= 0) {
            Die();
        }
    }
}
