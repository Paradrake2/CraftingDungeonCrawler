using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyProjectile : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileDamage;
    public Vector2 projectileDirection;
    public void Initialize(Vector2 direction, float damage, float speed) {
        projectileDirection = direction.normalized;
        projectileDamage = damage;
        projectileSpeed = speed;
    }

    void Update()
    {
        transform.Translate(projectileDirection * projectileSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            collision.GetComponent<Player>()?.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }

}

