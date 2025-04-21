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
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Hit layer: " + LayerMask.LayerToName(other.gameObject.layer));
        
        EnemyStats enemy = other.GetComponent<EnemyStats>();
        if (hitEnemies.Contains(enemy)) return;
        hitEnemies.Add(enemy);
        other.GetComponent<EnemyStats>()?.TakeDamage(playerStats.CurrentDamage);
        Vector2 knockbackDirection = other.transform.position - playerTransform.position;
        float knockbackForce = playerStats.CurrentKnockback * other.GetComponent<EnemyStats>().getKnockbackResistance();
        Debug.Log("hit enemy");
        other.GetComponent<EnemyStats>()?.ApplyKnockback(knockbackDirection, knockbackForce, knockbackTime);
            
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
