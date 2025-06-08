using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI_Beam : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 15f;
    public float standOffRadius = 7.5f;
    public float attackCooldown;
    private float lastAttackTime;
    public EnemyStats stats;

    public GameObject beam = null;
    public GameObject beamPreview = null;

    private bool isFiring = false;
    [SerializeField] private EnemyMovement enemyMovement;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Attack()
    {
        isFiring = true;
        GameObject beamLogic = new GameObject("BeamAttackLogic");
        LaserAttackController attackContoller = beamLogic.AddComponent<LaserAttackController>();
        if (attackContoller != null)
        {
            attackContoller.Initialize(stats.getDamage(), beamPreview, beam, player, transform.position);
            attackContoller.onBeamFinished = () => { isFiring = false; };
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
                enemyMovement.canMove = false;
            }
            else
            {
                enemyMovement.canMove = true;
            }
            enemyMovement.goBackwards = true;
        }
        else
        {
            enemyMovement.canMove = true;
        }
        enemyMovement.canMove = true;
    }
    private Vector3 getPlayerDirection() {
        Vector3 direction = (player.position - transform.position).normalized;
        return direction;
    }
    void MoveAwayFromPlayer() {
        transform.position -= getPlayerDirection() * (stats.getMovementSpeed() * 0.5f) *Time.deltaTime;
    }
}
