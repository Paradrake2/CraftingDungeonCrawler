using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
[System.Serializable]
public class EnvironmentalResourceEntry
{
    public GameObject prefab;
    public int baseRarityWeight;
    public int floorUnlock;
    public float weightPerFloor;
}
public class RoomGenerator : MonoBehaviour
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

    public RoomGenerator Instance;
    public Vector3 playerSpawnPosition;
    public Vector3 portalSpawnPosition;

    public void GenerateRoom()
    {
        var template = roomTemplates[UnityEngine.Random.Range(0, roomTemplates.Count)];
        int width = UnityEngine.Random.Range(template.minWidth, template.maxWidth);
        int height = UnityEngine.Random.Range(template.minHeight, template.maxHeight);

        float enemyDensity = Random.Range(template.minEnemyDensity, template.maxEnemyDensity);
        float obstacleDensity = Random.Range(template.minObstacleWeight, template.maxObstacleWeight);
        float oreDensity = Random.Range(template.minOreDensity, template.maxOreDensity);
        float chestDensity = Random.Range(template.minChestDensity, template.maxChestDensity);
        playerSpawnPosition = new Vector3(width / 2, height / 2, 0);
        portalSpawnPosition = new Vector3(width / 2, height / 2, 0);
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
                if (tilemap.HasTile(tilePos) && Random.value < obstacleDensity) Instantiate(obstaclePrefab[Random.Range(0, obstaclePrefab.Count())], spawnPos, Quaternion.identity);
                if (tilemap.HasTile(tilePos) && Random.value < oreDensity) SpawnOre(spawnPos);
                if (tilemap.HasTile(tilePos) && Random.value < chestDensity) Instantiate(envLoot[Random.Range(0, envLoot.Length)], spawnPos, Quaternion.identity);
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

    /*
    public Tilemap tilemap;
    public GameObject[] wallTile;
    public TileBase[] floorTile;
    public GameObject[] obstaclePrefabs; // have colliders
    public GameObject[] enemySpawner; // spawn enemy
    public EnvironmentalResourceEntry[] environmentalResourcePrefabs;
    public GameObject[] environmentalLoot;
    public EnemySpawn enemySpawn;
    public int radius = 30;
    
    public List<Vector3> enemySpawnPoints = new List<Vector3>();
    

    void Start() {
        //GenerateRoom();
    }

    public void GenerateRoom() {
        Vector2 center = new Vector2(0,0);
        GenerateWalls(center);
        GenerateContents(center);
        
        
    }
    void GenerateWalls(Vector2 center) {
        for (int x = -radius; x <= radius; x++) {
            for (int y = -radius; y <= radius; y++) {
                Vector2Int gridPos = new Vector2Int(x,y);
                Vector3Int tilePos = new Vector3Int(x,y,0);
                float distSq = Vector2.SqrMagnitude(gridPos - center);

                // Floor within circle
                if (distSq <= radius * radius) {
                    TileBase floor = floorTile[Random.Range(0, floorTile.Length)];
                    tilemap.SetTile(tilePos, floor);
                }
                
                // Walls around edge
                if (distSq >= (radius - 1) * (radius - 1) && distSq <= radius * radius) {
                    Vector3 spawnpos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f,0.5f,0);
                    GameObject wall = Instantiate(wallTile[Random.Range(0, wallTile.Length)], spawnpos, Quaternion.identity);
                    wall.transform.parent = this.transform;
                    
                }
            }
        }
    }
    void GenerateContents(Vector2 center) {
        for (int x = -radius + 1; x <= radius - 1; x++) {
            for (int y = -radius + 1; y <= radius - 1; y++) {
                Vector2Int gridPos = new Vector2Int(x,y);
                Vector3Int tilePos = new Vector3Int(x,y,0);

                if (Vector2.SqrMagnitude(gridPos - center) <= (radius - 2) * (radius - 2)) {
                    // check for floor tile
                    if (tilemap.HasTile(tilePos) && Random.value < 0.09f) {
                        spawnEnemySpawnPoint(tilePos);
                    }
                    if (tilemap.HasTile(tilePos) && Random.value < 0.06f) {
                        spawnObstacle(tilePos);
                    }
                    if (tilemap.HasTile(tilePos) && Random.value < 0.005f) {
                        spawnEnvResource(tilePos);
                    }
                    if (tilemap.HasTile(tilePos) && Random.value < 0.001f) {
                        spawnEnvLoot(tilePos);
                    }
                }
            }
        }
        foreach (Vector3 pos in enemySpawnPoints) {
            enemySpawn.SpawnEnemy(pos);
        }
    }
    private void spawnEnemySpawnPoint(Vector3Int tilePos) {
        Vector3 spawnpos = tilemap.CellToWorld(tilePos) + new Vector3(0,0,0);
        enemySpawnPoints.Add(spawnpos);
    }
    private void spawnObstacle(Vector3Int tilePos) {
        Vector3 spawnpos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f,0.5f, 0);
        GameObject obstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], spawnpos, Quaternion.identity);
        obstacle.transform.parent = this.transform;
    }
    private void spawnEnvResource(Vector3Int tilePos) {
        Vector3 spawnPos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f,0.5f,0);
        GameObject prefabToSpawn = GetRandomResourceByRarity(DungeonManager.Instance.getFloor());
        GameObject envResource = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        //GameObject envResource = Instantiate(environmentalResourcePrefabs[Random.Range(0, environmentalResourcePrefabs.Length)], spawnPos, Quaternion.identity);
        envResource.transform.parent = this.transform;
    }
    private void spawnEnvLoot(Vector3Int tilePos) {
        Vector3 spawnPos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0.5f);
        GameObject envLoot = Instantiate(environmentalLoot[Random.Range(0,environmentalLoot.Length)], spawnPos, Quaternion.identity);
        envLoot.transform.parent = this.transform;
    }
    private GameObject GetRandomResourceByRarity(int floorNumber)
    {
        List<(GameObject prefab, float weight)> eligibleResource = new();
        foreach (var entry in environmentalResourcePrefabs)
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
        return environmentalResourcePrefabs[0].prefab; //fallback
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
    */
}

