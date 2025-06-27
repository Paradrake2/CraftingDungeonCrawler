using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSlash : MonoBehaviour
{
    public float lifetime = 0.2f;
    public float knockbackTime = 0.25f;
    public LayerMask enemyLayer;
    private Transform playerTransform;
    public PlayerStats playerStats;
    private HashSet<EnemyStats> hitEnemies = new HashSet<EnemyStats>();
    void Start()
    {
        //playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerStats = PlayerStats.Instance;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        EnemyStats enemy = other.GetComponent<EnemyStats>();
        if (enemy != null)
        {
            if (hitEnemies.Contains(enemy)) return;
            hitEnemies.Add(enemy);
            other.GetComponent<Enemy>()?.TakeDamage(playerStats.CurrentDamage);
            Vector2 knockbackDirection = other.transform.position - playerTransform.position;
            float knockbackForce = playerStats.CurrentKnockback * other.GetComponent<EnemyStats>().getKnockbackResistance();
            other.GetComponent<EnemyStats>()?.ApplyKnockback(knockbackDirection, knockbackForce, knockbackTime);
        }
        if (other.tag == "EnvironmentalResource")
        {
            Debug.LogWarning("Hit env resource");
        }
        Obstacle obs = other.GetComponent<Obstacle>();
        if (obs != null)
        {
            obs.ObstacleDamage(playerStats.CurrentDamage);
            Debug.Log("Hit obstacle");
        }
    }
}
