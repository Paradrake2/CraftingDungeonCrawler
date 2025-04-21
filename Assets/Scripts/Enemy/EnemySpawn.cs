using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawn : MonoBehaviour
{
    private EnemyRarity ChooseRarity() {
        float roll = Random.value;
        if (roll <= 0.01f) return EnemyRarity.Legendary;
        else if (roll <= 0.11f) return EnemyRarity.Rare;
        else return EnemyRarity.Common;
    }
    public List<EnemyArea> areaEnemyLists;
    public void SpawnEnemy(Vector3 position) {
        int areaIndex = DungeonManager.Instance.floor/5;
        areaIndex = Mathf.Clamp(areaIndex,0,areaEnemyLists.Count - 1);
        EnemyArea area = areaEnemyLists[areaIndex];
        var rarityGroups = area.rarityGroups;
        EnemyRarity selectedRarity = ChooseRarity();
        var group = rarityGroups.FirstOrDefault(gameObject => gameObject.rarity == selectedRarity);
        if (group == null) {
            Debug.LogError("EnemyRarityGroup is null!");
            return;
        }
        if (group == null || group.enemies.Count == 0) {
            // Fall back to common enemies
            Debug.LogWarning("hhhhh");
            group = rarityGroups.FirstOrDefault(g => g.rarity == EnemyRarity.Common);
            if (group == null || group.enemies.Count == 0) {
                Debug.LogWarning("No valid enemies to spawn in area " + areaIndex);
                return;
            }
        }
        GameObject chosenEnemy = group.enemies[Random.Range(0, group.enemies.Count)];
        Debug.Log(chosenEnemy + "  " + group.rarity);
        //int attempts = 0;
        //while (chosenEnemy == null && attempts < 10) {
        //    chosenEnemy = group.enemies[Random.Range(0, group.enemies.Count)];
        //    Debug.Log(chosenEnemy);
        //    attempts++;
        //}
        if (chosenEnemy == null) {
            chosenEnemy = group.enemies[0];
            Debug.LogWarning("was null");
        }
        Instantiate(chosenEnemy, position, Quaternion.identity);
    }
}
