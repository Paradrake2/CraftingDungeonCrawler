using System;
using System.Collections;
using UnityEngine;

[System.Serializable]

public class Combat : MonoBehaviour
{
    public float swingDuration = 0.5f;
    public float slashDistance = 1f;
    public LayerMask enemyLayer;

    private bool isSwinging = false;
    private PlayerStats playerStats;
    
    public GameObject slashPrefab;

    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            Vector2 direction = (mouseWorld - transform.position).normalized;

            // calculate spawn position offset from player
            Vector3 spawnPos = transform.position + (Vector3)direction * slashDistance;

            // Rotate slash to face the same direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0,0,angle);
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, direction);

            Instantiate(slashPrefab, spawnPos, rotation);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            other.GetComponent<Enemy>()?.TakeDamage(playerStats.CurrentDamage);
        }
    }
}
