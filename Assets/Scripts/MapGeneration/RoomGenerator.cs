using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class RoomGenerator : MonoBehaviour {
    public Tilemap tilemap;
    public GameObject[] wallTile;
    public TileBase[] floorTile;
    public GameObject[] obstaclePrefabs; // have colliders
    public GameObject[] enemySpawner; // spawn enemy
    public GameObject[] environmentalResourcePrefabs;
    public EnemySpawn enemySpawn;
    public int radius = 30;
    
    public List<Vector3> enemySpawnPoints = new List<Vector3>();
    

    void Start() {
        //GenerateRoom();
    }

    public void GenerateRoom() {
        Vector2 center = new Vector2(0,0);

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
        for (int x = -radius + 1; x <= radius - 1; x++) {
            for (int y = -radius + 1; y <= radius - 1; y++) {
                Vector2Int gridPos = new Vector2Int(x,y);
                Vector3Int tilePos = new Vector3Int(x,y,0);

                if (Vector2.SqrMagnitude(gridPos - center) <= (radius - 2) * (radius - 2)) {
                    // check for floor tile
                    if (tilemap.HasTile(tilePos) && Random.value < 0.08f) {
                        Vector3 spawnpos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f,0.5f, 0);
                        GameObject obstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], spawnpos, Quaternion.identity);
                        obstacle.transform.parent = this.transform;
                    }
                    if (tilemap.HasTile(tilePos) && Random.value < 0.09f) {
                        Vector3 spawnpos = tilemap.CellToWorld(tilePos) + new Vector3(0,0,0);
                        enemySpawnPoints.Add(spawnpos);
                        Debug.DrawLine(spawnpos, spawnpos + Vector3.up * 0.5f, Color.red, 5f);
                    }
                    if (tilemap.HasTile(tilePos) && Random.value < 0.005f) {
                        Vector3 spawnPos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f,0.5f,0);
                        GameObject envResource = Instantiate(environmentalResourcePrefabs[Random.Range(0, environmentalResourcePrefabs.Length)], spawnPos, Quaternion.identity);
                        envResource.transform.parent = this.transform;
                    }

                }
            }
        }
        foreach (Vector3 pos in enemySpawnPoints) {
            Debug.Log(pos);
            enemySpawn.SpawnEnemy(pos);
        }
        
    }

    public void ClearRoom() {
        tilemap.ClearAllTiles();
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        enemySpawnPoints.Clear();
    }
}

