using System.Collections;
using UnityEngine;

public class EnemySlash : MonoBehaviour
{
    public float animationTime;
    public float slashDamage;
    private bool hasHitPlayer = false;
    private EnemyStats enemyStats;
    void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        //slashDamage = enemyStats.damage;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasHitPlayer)
        {
            hasHitPlayer = true;
            other.GetComponent<Player>()?.TakeDamage(slashDamage);
        }
    }
}
