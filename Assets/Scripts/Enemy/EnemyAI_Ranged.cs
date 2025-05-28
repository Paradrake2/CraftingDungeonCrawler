using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI_Ranged : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 15f;
    public float standOffRadius = 7.5f;
    public float attackCooldown;
    private float lastAttackTime;
    public float projectileSpeed;
    public EnemyStats stats;
    public GameObject projectile;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Attack() {
        Vector3 direction = getPlayerDirection();

        GameObject proj = Instantiate(projectile, transform.position, quaternion.identity);
        EnemyProjectile projScript = proj.GetComponent<EnemyProjectile>();
        if (projScript != null) {
            projScript.Initialize(direction, stats.getDamage(), projectileSpeed);
        }
    }
    void Update()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);
        
        if (distance <= detectionRadius)
        {
            lastAttackTime -= Time.deltaTime;
            if (lastAttackTime <= 0f)
            {
                Attack();
                lastAttackTime = attackCooldown;
            }
            MoveTowardsPlayer();
            if (distance <= standOffRadius)
            {
                MoveAwayFromPlayer();
            }
        }
    }
    private Vector3 getPlayerDirection() {
        Vector3 direction = (player.position - transform.position).normalized;
        return direction;
    }
    void MoveTowardsPlayer()
    {
        transform.position += getPlayerDirection() * stats.getMovementSpeed() * Time.deltaTime;
        Vector2 direction = player.position - transform.position;
        if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }
    void MoveAwayFromPlayer() {
        transform.position -= getPlayerDirection() * (stats.getMovementSpeed() * 0.5f) *Time.deltaTime;
    }
}
