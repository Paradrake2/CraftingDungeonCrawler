using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyArea
{
    public string areaName;
    public List<EnemyRarityGroup> rarityGroups;
}
[System.Serializable]
public class EnemyRarityGroup {
    public EnemyRarity rarity;
    public List<GameObject> enemies;
}