using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileDamage;
    public float projectileSize;
    public Vector2 projectileDirection;
    public Transform spriteTransform;
    public float aoeRadius;
    private Rigidbody2D rb;
    public int piercingAmount;
    public float timeoutTime; // this will destroy the gameobject after a certain amount of time

    private int piercedNum;
    public void Initialize(Vector2 direction, float damage, float speed)
    {
        projectileDirection = direction.normalized;
        projectileDamage = damage;
        projectileSpeed = speed;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = projectileDirection * projectileSpeed;

        float angle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        if (spriteTransform != null) spriteTransform.localRotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) {
            collision.GetComponent<Enemy>()?.TakeDamage(projectileDamage);
            piercedNum++;
            if (piercedNum >= piercingAmount) Destroy(gameObject);
        }
    }
    public Vector2 getDirection()
    {
        return projectileDirection;
    }
}
