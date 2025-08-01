using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyAI_Melee : MonoBehaviour
{
    public Transform player;
    public float attackCooldown;
    private float lastAttackTime;

    public EnemyStats stats;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
    }
    /*
    void MoveTowardsPlayer() {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector2 directionS = player.position - transform.position;
        if (directionS.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (directionS.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        transform.position += direction * stats.getMovementSpeed() * Time.deltaTime;
    }
    */

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown) {
            lastAttackTime = Time.time;
            if (stats != null) {
                other.GetComponent<Player>()?.TakeDamage(stats.damage);
            } else {
                Debug.Log("No enemy stats attached to this enemy!");
            }
        }
    }
}
