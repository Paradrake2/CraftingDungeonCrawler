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
    public GameObject projectile = null;

    public GameObject beam = null;
    public GameObject beamPreview = null;
    public bool isBeamAttack;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Attack()
    {
        Vector3 direction = getPlayerDirection();
        if (!isBeamAttack) ProjectileAttack(direction);
        if (isBeamAttack) BeamAttack();

    }
    void ProjectileAttack(Vector3 direction)
    {
        GameObject proj = Instantiate(projectile, transform.position, quaternion.identity);
        EnemyProjectile projScript = proj.GetComponent<EnemyProjectile>();
        if (projScript != null) {
            projScript.Initialize(direction, stats.getDamage(), projectileSpeed);
        }
    }

    void BeamAttack()
    {
        GameObject beamLogic = new GameObject("BeamAttackLogic");
        LaserAttackController attackContoller = beamLogic.AddComponent<LaserAttackController>();
        if (attackContoller != null)
        {
            attackContoller.Initialize(stats.getDamage(), beamPreview, beam, player, transform.position);
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
            //MoveTowardsPlayer();
            
        } else if (distance <= standOffRadius)
        {
            //MoveAwayFromPlayer();
        }
    }
    private Vector3 getPlayerDirection() {
        Vector3 direction = (player.position - transform.position).normalized;
        return direction;
    }

}
