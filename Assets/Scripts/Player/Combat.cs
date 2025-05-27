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
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void Update()
    {

        if (playerStats.GetEquippedItem(EquipmentSlot.Weapon) == null || playerStats.GetEquippedItem(EquipmentSlot.Weapon).weaponType == WeaponType.Melee)
        {
            if (Input.GetMouseButtonDown(0) && !isSwinging)
            {
                SlashBasic();
            }
        }
        else if (playerStats.GetEquippedItem(EquipmentSlot.Weapon).weaponType == WeaponType.Magic)
        {
            if (Input.GetMouseButtonDown(0)) FireMagicProjectile();
        }


    }
    void SlashBasic()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2 direction = (mouseWorld - transform.position).normalized;
        // calculate spawn position offset from player
        Vector3 spawnPos = transform.position + (Vector3)direction * slashDistance;
        // Rotate slash to face the same direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, direction);
        Instantiate(slashPrefab, spawnPos, rotation);
    }

    void FireMagicProjectile()
    {
        GameObject proj = PlayerStats.Instance.GetEquippedItem(EquipmentSlot.Weapon).GetProjectile();
        float speed = PlayerStats.Instance.GetEquippedItem(EquipmentSlot.Weapon).GetProjectileSpeed();
        if (proj == null)
        {
            Debug.LogError("NO PROJECTILE PREFAB");
            return;
        }

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2 direction = (mouseWorld - transform.position).normalized;
        Vector3 spawnPos = transform.position + (Vector3)direction;

        if (HasEnoughMana(playerStats.GetEquippedItem(EquipmentSlot.Weapon))) {
            playerStats.CurrentMana -= playerStats.GetEquippedItem(EquipmentSlot.Weapon).GetManaCost();
            GameObject projObj = Instantiate(proj, spawnPos, Quaternion.identity);
            PlayerProjectile plProj = projObj.GetComponent<PlayerProjectile>();
            if (plProj != null) plProj.Initialize(direction, playerStats.CurrentDamage, speed);
        }

    }

    bool HasEnoughMana(Equipment weapon)
    {
        float currentMana = playerStats.CurrentMana;
        float manaCost = weapon.GetManaCost();
        if (currentMana > manaCost) return true;
        return false;
    }
}
