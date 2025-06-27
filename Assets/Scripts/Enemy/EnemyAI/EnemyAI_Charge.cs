using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyAI_Charge : EnemyMovement
{
    [SerializeField] private float movementMult;
    [SerializeField] private float chargeRange;
    [SerializeField] private float chargeTime;
    private bool isCharging = false;
    public float attackCooldown;
    private float lastAttackTime;
    public override void UpdateMovement()
    {
        if (player == null || isCharging) return;

        if (InRange())
        {
            if (CanSeePlayer() && InChargeRange() && !isCharging)
            {
                StartCoroutine(ChargeRoutine());
            }
            else
            {
                Pathfind();
            }
        }
    }

    private IEnumerator ChargeRoutine()
    {
        animator.SetBool("isCharging", true);
        isCharging = true;

        Vector2 direction = (player.position - transform.position).normalized;

        float duration = chargeTime;
        float timer = 0f;
        HandleSpriteDirection(Direction());
        while (timer < duration && !HasHitObstacle())
        {
            if (HasHitObstacle())
            {
                animator.SetBool("isCharging", false);
                isCharging = false;
                StartCoroutine(StaggerRoutine());
                yield break;
            }

            //transform.position += Direction() * GetSpeed() * Time.deltaTime;
            rb.MovePosition(rb.position + direction * GetSpeed() * Time.fixedDeltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        canMove = false;
        yield return new WaitForSeconds(attackCooldown);
        canMove = true;

        animator.SetBool("isCharging", false);
        isCharging = false;
    }


    private IEnumerator StaggerRoutine()
    {
        return null;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= lastAttackTime * attackCooldown && isCharging)
        {
            lastAttackTime = Time.time;
            if (stats != null)
            {
                other.GetComponent<Player>()?.TakeDamage(stats.damage);
            }
            else
            {
                Debug.LogError($"NO STATS ATTACHED TO THIS ENEMY {stats.name}");
            }
        }
    }

    bool InChargeRange()
    {
        return GetDistance() < chargeRange;
    }
    float GetSpeed()
    {
        return stats.movementSpeed * movementMult;
    }


    bool HasHitObstacle()
    {
        LayerMask obstacleMask = LayerMask.GetMask("Obstacle");
        return CheckForHit(obstacleMask);
    }
    bool CheckForHit(LayerMask layerMask)
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.2f, layerMask);
        return hit != null;
    }
}
