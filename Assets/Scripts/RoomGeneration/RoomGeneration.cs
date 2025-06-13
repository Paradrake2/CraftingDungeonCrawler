using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGeneration : MonoBehaviour
{
    public List<RoomTemplate> roomTemplates;
    public Tilemap tilemap;
    public TileBase[] floorPrefab;
    public GameObject[] wallPrefab;
    public GameObject[] obstaclePrefab;
    public EnvironmentalResourceEntry[] orePrefab;
    public GameObject[] envLoot;
    public EnemySpawn enemySpawn;
    public List<Vector3> enemySpawnPoints = new List<Vector3>();

    public RoomGeneration Instance;


    public void GenerateRoom()
    {
        var template = roomTemplates[UnityEngine.Random.Range(0, roomTemplates.Count)];
        int width = UnityEngine.Random.Range(template.minWidth, template.maxWidth);
        int height = UnityEngine.Random.Range(template.minHeight, template.maxHeight);

        float enemyDensity = Random.Range(template.minEnemyDensity, template.maxOreDensity);
        float obstacleDensity = Random.Range(template.minObstacleWeight, template.maxObstacleWeight);
        float oreDensity = Random.Range(template.minOreDensity, template.maxOreDensity);
        float chestDensity = Random.Range(template.minChestDensity, template.maxChestDensity);
        BuildRoomBounds(width, height, enemyDensity, obstacleDensity, oreDensity, chestDensity);
    }
    public void ClearRoom()
    {
        tilemap.ClearAllTiles();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        enemySpawnPoints.Clear();
    }
    void BuildRoomBounds(int width, int height, float enemyDensity, float obstacleDensity, float oreDensity, float chestDensity)
    {
        for (int x = -1; x <= width; x++)
        {
            for (int y = -1; y <= height; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                CreateWalls(x, y, width, height, tilePos);
                CreateFloor(tilePos);
            }
        }
        GenerateContents(width, height, enemyDensity, obstacleDensity, oreDensity, chestDensity);
    }
    void CreateWalls(int x, int y, int width, int height, Vector3Int tilePos)
    {

        if (x == -1 || y == -1 || x == width || y == height)
        {
            Vector3 spawnPos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0);
            Instantiate(wallPrefab[Random.Range(0, wallPrefab.Count())], spawnPos, Quaternion.identity);
        }
    }
    void CreateFloor(Vector3Int tilePos)
    {
        TileBase floor = floorPrefab[Random.Range(0, floorPrefab.Length)];
        tilemap.SetTile(tilePos, floor);
    }
    void GenerateContents(int width, int height, float enemyDensity, float obstacleDensity, float oreDensity, float chestDensity)
    {
        for (int x = 0; x <= width - 1; x++)
        {
            for (int y = 0; y <= height - 1; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                Vector3 spawnPos = new Vector3(x, y, 0) + new Vector3(0.5f, 0.5f, 0);
                if (tilemap.HasTile(tilePos) && Random.value < enemyDensity) enemySpawnPoints.Add(spawnPos);
                if (tilemap.HasTile(tilePos) && Random.value < obstacleDensity) Instantiate(obstaclePrefab[Random.Range(0, obstaclePrefab.Count())]);
                if (tilemap.HasTile(tilePos) && Random.value < oreDensity) SpawnOre(spawnPos);
                if (tilemap.HasTile(tilePos) && Random.value < chestDensity) Instantiate(envLoot[Random.Range(0, envLoot.Length)]);
            }
        }
        foreach (Vector3 pos in enemySpawnPoints)
        {
            enemySpawn.SpawnEnemy(pos);
        }
    }
    void SpawnOre(Vector3 spawnPos)
    {
        GameObject prefabToSpawn = GetRandomResourceByRarity(DungeonManager.Instance.getFloor());
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
    private GameObject GetRandomResourceByRarity(int floorNumber)
    {
        List<(GameObject prefab, float weight)> eligibleResource = new();
        foreach (var entry in orePrefab)
        {
            if (floorNumber >= entry.floorUnlock)
            {
                float adjustedWeight = entry.baseRarityWeight + (floorNumber - entry.floorUnlock) * entry.weightPerFloor;
                eligibleResource.Add((entry.prefab, adjustedWeight));
            }
        }
        float totalWeight = eligibleResource.Sum(e => e.weight);
        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var (prefab, weight) in eligibleResource)
        {
            cumulative += weight;
            if (roll <= cumulative) return prefab;
        }
        return orePrefab[0].prefab; //fallback
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
