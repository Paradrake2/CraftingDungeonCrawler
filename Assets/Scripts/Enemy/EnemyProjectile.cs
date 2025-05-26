using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyProjectile : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileDamage;
    public Vector2 projectileDirection;
    public Transform spriteTransform;
    public void Initialize(Vector2 direction, float damage, float speed)
    {
        projectileDirection = direction.normalized;
        projectileDamage = damage;
        projectileSpeed = speed;
        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        if (spriteTransform != null) spriteTransform.localRotation = Quaternion.Euler(0f, 0f, angle-90f);
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
    public Vector2 getDirection()
    {
        return projectileDirection;
    }
}

