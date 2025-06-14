using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public PathfindingNode[,] grid;
    public int gridResolution = 2;
    private bool[,] map;

    public void GenerateRoom()
    {
        var template = roomTemplates[UnityEngine.Random.Range(0, roomTemplates.Count)];
        int width = UnityEngine.Random.Range(template.minWidth, template.maxWidth);
        int height = UnityEngine.Random.Range(template.minHeight, template.maxHeight);

        float enemyDensity = Random.Range(template.minEnemyDensity, template.maxEnemyDensity);
        float obstacleDensity = Random.Range(template.minObstacleWeight, template.maxObstacleWeight);
        float oreDensity = Random.Range(template.minOreDensity, template.maxOreDensity);
        float chestDensity = Random.Range(template.minChestDensity, template.maxChestDensity);

        float noise = template.noise;
        int iterations = template.iterations;

        Debug.Log($"Width: {width}, height: {height}");
        map = new bool[width, height];
        //BuildRoomBounds(width, height, enemyDensity, obstacleDensity, oreDensity, chestDensity);
        if (template.roomLayout == RoomLayout.Cellular) GenerateRandomMapCellular(width, height, noise, iterations, enemyDensity, obstacleDensity, oreDensity, chestDensity);
        // generate different room types

        // create pathfinding nodes for enemies

        // player and portal spawn positions
        playerSpawnPosition = GenerateRandomPosition();
        portalSpawnPosition = GenerateRandomPosition();
        //OnDrawGizmos();
    }
/*
    public void OnDrawGizmos()
    {
        if (grid == null)
            return;

        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        float cellSize = 1f / gridResolution;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var node = grid[x, y];
                Vector3 worldPos = new Vector3((x + 0.5f)* cellSize, (y + 0.5f)*cellSize, 0);

                Gizmos.color = node.walkable ? Color.green : Color.red;
                Gizmos.DrawCube(worldPos, Vector3.one * cellSize * 0.9f);
            }
        }
    }
*/
    void BuildNodeGrid()
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        grid = new PathfindingNode[width * gridResolution, height * gridResolution];

        for (int x = 0; x < width * gridResolution; x++)
        {
            for (int y = 0; y < height * gridResolution; y++)
            {
                int mapX = x / gridResolution;
                int mapY = y / gridResolution;

                bool walkable = map[mapX, mapY];
                grid[x, y] = new PathfindingNode(x, y, walkable);
            }
        }
    }

    // Cellular map gen
    public void GenerateRandomMapCellular(int width, int height, float noise, int iterations, float enemyDensity, float obstacleDensity, float oreDensity, float chestDensity)
    {
        RandomFill(width, height, noise);

        // simulation steps
        for (int i = 0; i < iterations; i++)
        {
            map = SimulationStep(map, width, height);
        }

        map = FloodFillLargestRegion(map, width, height); // cleans up the map
        BuildNodeGrid();

        // generate the actual map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (map[x, y])
                {
                    tilemap.SetTile(pos, floorPrefab[Random.Range(0, floorPrefab.Count())]); // create floor
                    GenerateContents(pos, enemyDensity, obstacleDensity, oreDensity, chestDensity);
                }
                else
                {
                    Instantiate(wallPrefab[Random.Range(0, wallPrefab.Length)], pos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                }
            }
        }
        foreach (Vector3 pos in enemySpawnPoints)
        {
            enemySpawn.SpawnEnemy(pos);
        }
    }
    void RandomFill(int width, int height, float noise)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1) map[x, y] = false; // border is always walls
                else map[x, y] = UnityEngine.Random.value > noise;
            }
        }
    }

    bool[,] SimulationStep(bool[,] oldMap, int width, int height)
    {
        Debug.Log("Called sim step");
        bool[,] newMap = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbors = CountWallNeighbors(oldMap, x, y, width, height);
                if (neighbors > 4) newMap[x, y] = false; // wall
                else if (neighbors < 4) newMap[x, y] = true; // floor
                else newMap[x, y] = oldMap[x, y];
            }
        }
        return newMap;
    }
    int CountWallNeighbors(bool[,] map, int x, int y, int width, int height)
    {
        int count = 0;
        for (int nx = x - 1; nx <= x + 1; nx++)
        {
            for (int ny = y - 1; ny <= y + 1; ny++)
            {
                if (nx == x && ny == y) continue;
                if (nx < 0 || ny < 0 || nx >= width || ny >= height) count++;
                else if (!map[nx, ny]) count++; // count the area outside the height and width as walls
            }
        }
        return count;
    }

    bool[,] FloodFillLargestRegion(bool[,] map, int width, int height)
    {
        bool[,] hasVisited = new bool[width, height];
        List<HashSet<(int, int)>> regions = new();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] && !hasVisited[x, y])
                {
                    HashSet<(int, int)> region = new();
                    Queue<(int, int)> queue = new();
                    queue.Enqueue((x, y));
                    hasVisited[x, y] = true;

                    while (queue.Count > 0)
                    {
                        var (cx, cy) = queue.Dequeue();
                        region.Add((cx, cy));

                        foreach (var (nx, ny) in GetNeighbors(cx, cy, width, height))
                        {
                            if (!hasVisited[nx, ny] && map[nx, ny])
                            {
                                queue.Enqueue((nx, ny));
                                hasVisited[nx, ny] = true;
                            }
                        }
                    }
                    regions.Add(region);
                }

            }
        }

        var largestRegion = regions.OrderByDescending(r => r.Count).FirstOrDefault();

        // build the cleaned up map
        bool[,] cleaned = new bool[width, height];
        foreach (var (x, y) in largestRegion) cleaned[x, y] = true;

        return cleaned;
    }

    List<(int, int)> GetNeighbors(int x, int y, int width, int height) {
        List<(int, int)> neighbors = new();
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };
        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i], ny = y + dy[i];
            if (nx >= 0 && ny >= 0 && nx < width && ny < height) neighbors.Add((nx, ny));
        }
        return neighbors;
    }




    // Content generation
    void GenerateContents(Vector3Int tilePos, float enemyDensity, float obstacleDensity, float oreDensity, float chestDensity)
    {
        Vector3 spawnPos = new Vector3(tilePos.x, tilePos.y, 0) + new Vector3(0.5f, 0.5f, 0);
        if (tilemap.HasTile(tilePos) && Random.value < enemyDensity) enemySpawnPoints.Add(spawnPos);
        if (tilemap.HasTile(tilePos) && Random.value < obstacleDensity) CreateObstacle(tilePos, spawnPos);
        if (tilemap.HasTile(tilePos) && Random.value < oreDensity) SpawnOre(spawnPos, tilePos);
        if (tilemap.HasTile(tilePos) && Random.value < chestDensity) Instantiate(envLoot[Random.Range(0, envLoot.Length)], spawnPos, Quaternion.identity);
    }
    void CreateObstacle(Vector3 tilePos, Vector3 spawnPos)
    {
        Instantiate(obstaclePrefab[Random.Range(0, obstaclePrefab.Count())], spawnPos, Quaternion.identity);
        MakeUnwalkable(tilePos);
    }
    void SpawnOre(Vector3 spawnPos, Vector3 tilePos)
    {
        GameObject prefabToSpawn = GetRandomResourceByRarity(DungeonManager.Instance.getFloor());
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        MakeUnwalkable(tilePos);
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

    void MakeUnwalkable(Vector3 tilePos)
    {
        map[(int)tilePos.x, (int)tilePos.y] = false;

        int mapX = (int)tilePos.x;
        int mapY = (int)tilePos.y;

        for (int subX = 0; subX < gridResolution; subX++)
        {
            for (int subY = 0; subY < gridResolution; subY++)
            {
                int gridX = mapX * gridResolution + subX;
                int gridY = mapY * gridResolution + subY;
                grid[gridX, gridY].walkable = false;
            }
        }
    }





    // ------------------------------------- Misc -------------------------------------
    public void ClearRoom()
    {
        tilemap.ClearAllTiles();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        enemySpawnPoints.Clear();
    }
    // get random floor tile
    Vector3 GenerateRandomPosition()
    {
        List<Vector3Int> floorPositions = new();

        int width = map.GetLength(0);
        int height = map.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y]) floorPositions.Add(new Vector3Int(x, y, 0));
            }
        }

        if (floorPositions.Count == 0)
        {
            Debug.LogError("No valid floor tiles for player spawn!");
            return Vector3.zero;

        }
        Vector3Int selectedTile = floorPositions[UnityEngine.Random.Range(0, floorPositions.Count)];
        return new Vector3(selectedTile.x + 0.5f, selectedTile.y + 0.5f, 0);
    }
    
    /*
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
        // GenerateContents(width, height, enemyDensity, obstacleDensity, oreDensity, chestDensity);
    }
    void CreateWalls(int x, int y, int width, int height, Vector3Int tilePos)
    {

        if (x == -1 || y == -1 || x == width || y == height)
        {
            Vector3 spawnPos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0);
        }
    }
    void CreateFloor(Vector3Int tilePos)
    {
        TileBase floor = floorPrefab[Random.Range(0, floorPrefab.Length)];
        tilemap.SetTile(tilePos, floor);
    }
    */
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

