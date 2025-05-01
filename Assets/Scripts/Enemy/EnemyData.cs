using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;
    public float maxHealth;
    public float damage;
    public float moveSpeed;
    public float attackCooldown;
    public float defense;

    public float minXPDrop;
    public float maxXPDrop;
    public float minGoldDrop;
    public float maxGoldDrop;

    public Sprite icon;
    public EnemyRarity rarity;
}
