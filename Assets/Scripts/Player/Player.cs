using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private PlayerStats stats;
    public float damageCooldown = 0.1f;
    private float lastDamageTime = -999f;
    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }
    public void TakeDamage(float damage) {
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;
        float finalDamage = Mathf.Max(0, damage - stats.CurrentDefense);
        stats.CurrentHealth -= finalDamage;

        if (stats.CurrentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Player has died");
        SceneManager.LoadScene("Menu");
        //SceneManager.UnloadSceneAsync("Dungeon");
        // Bring up menu where crafting is available
    }

}
