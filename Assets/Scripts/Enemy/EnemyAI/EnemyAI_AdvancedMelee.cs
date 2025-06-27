using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyAI_AdvancedMelee : MonoBehaviour
{
    //public EnemySlash enemySlash;
    public GameObject enemySlashPrefab;
    public GameObject enemySlashPreviewPrefab = null;
    public GameObject blockPrefab;
    public GameObject blockPreviewPrefab;
    public float attackRange;
    public float attackCooldown;
    public float previewTime;
    private float timeSinceLastAttack;
    public EnemyStats enemyStats;
    public Player player;
    public EnemyMovement enemyMovement;

    public Animator attackAnimator = null;
    public float rotateSlash;
    public bool isAttacking = false;
    public void Attack()
    {
        Vector3 startPos = transform.position;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        StartCoroutine(AttackRoutine(startPos, enemyStats.damage, direction));
    }

    IEnumerator AttackRoutine(Vector3 startPos, float damage, Vector2 direction)
    {
        attackAnimator.SetBool("isAttacking", true);
        enemyMovement.canMove = false;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotateSlash;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 spawnPos = startPos + (Vector3)direction * attackRange;

        if (enemySlashPreviewPrefab != null)
        {
            GameObject preview = Instantiate(enemySlashPreviewPrefab, spawnPos, rotation);
            if (preview.TryGetComponent<TimedDestroy>(out var timedDestroy)) timedDestroy.destroyTime = previewTime;
            yield return new WaitForSeconds(previewTime);
        }

        GameObject slash = Instantiate(enemySlashPrefab, spawnPos, rotation);
        if (slash.GetComponent<EnemySlash>() != null)
        {
            float anTime = slash.GetComponent<EnemySlash>().animationTime;
            if (slash.TryGetComponent<TimedDestroy>(out var timedDestroy1)) timedDestroy1.destroyTime = anTime;
            yield return new WaitForSeconds(anTime);
        }
        attackAnimator.SetBool("isAttacking", false);
        enemyMovement.canMove = true;
        yield return new WaitForSeconds(attackCooldown);
    }
    

    bool IsInRange()
    {
        Vector2 direction = player.transform.position - transform.position;

        Vector2 rayOrigin = (Vector2)transform.position;
        LayerMask obstacleMask = LayerMask.GetMask("player");
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, obstacleMask);
        return hit.collider != null;
    }

    // block will be implemented later
    public void StartBlock()
    {

    }
    IEnumerator BlockRoutine()
    {
        return null; // placeholder to prevent error
    }
    
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInRange() && Time.time >= timeSinceLastAttack + attackCooldown)
        {
            Debug.Log("ATTACKING");
            Attack();
            timeSinceLastAttack = Time.time;
        }
    }
}
