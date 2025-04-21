using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyRarity {
    Common,
    Rare,
    Legendary,
    Boss
}
public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float damage;
    public float defense;
    public float knockback;
    public float knockbackResistance;
    public float movementSpeed;
    public bool knockedBack = false;
    public float knockedBackTime = 0.1f;
    public bool wasHitByPlayer = false;

    [Header("Drops")]
    public float minXP;
    public float maxXP;
    public float minGold;
    public float maxGold;
    public List<Items> possibleDrops = new List<Items>();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage) {
        currentHealth = currentHealth - (float)damage;
        wasHitByPlayer = true;
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration) {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) {
            StopCoroutine(KnockbackRoutine(direction, force, duration));
            StartCoroutine(WasHit());
            StartCoroutine(KnockbackRoutine(direction, force, duration));
            Debug.Log(force);
        }

        
    }

    public float getMovementSpeed() {
        return movementSpeed;
    }
    public IEnumerator WasHit() {
        if (knockedBack) yield break;
        knockedBack = true;
        float originalMoveSpeed = movementSpeed;
        movementSpeed = 0;
        yield return new WaitForSeconds(knockedBackTime);
        movementSpeed = originalMoveSpeed;
        knockedBack = false;
    }
    
    private IEnumerator KnockbackRoutine(Vector2 direction, float force, float duration) {
        float timer = 0f;
        while (timer < duration) {
            transform.position += (Vector3)(direction.normalized * force * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    
    }

    public float getKnockbackResistance() {
        return knockbackResistance;
    }

    public float getHealth() {
        return currentHealth;
    }
}
