using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStats stats;
    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }
     public void TakeDamage(float damage) {
        float finalDamage = Mathf.Max(0, damage - stats.CurrentDefense);
        stats.CurrentHealth -= finalDamage;

        if (stats.CurrentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Player has died");
        // Bring up menu where crafting is available
    }

}
