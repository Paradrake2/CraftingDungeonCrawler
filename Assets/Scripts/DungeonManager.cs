using UnityEngine;
using UnityEngine.SceneManagement;
public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    public RoomGenerator roomGenerator;
    public GameObject playerPrefab;
    public Vector3 playerSpawnPosition;
    public int totalEnemies = 0;
    public int enemiesKilled = 0;
    public int floor;

    public GameObject portalPrefab;

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    void Start() {
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Dungeon") {
            roomGenerator = GameObject.FindFirstObjectByType<RoomGenerator>();
            if (roomGenerator != null) {
                roomGenerator.ClearRoom();
                roomGenerator.GenerateRoom();
            }
            else {
                Debug.LogError("RoomGenerator not found in scene!");
            }
            GameObject player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
            Camera.main.GetComponent<CameraMov>().target = player.transform;
            totalEnemies = 0;
            enemiesKilled = 0;
        }
    }
    public void RegisterEnemy() {
        totalEnemies++;
    }

    public void OnEnemyKilled() {
        enemiesKilled++;
        if (enemiesKilled >= totalEnemies){
            Debug.Log("All enemies killed");
            SpawnPortal();
        }
    }

    void SpawnPortal() {
        Instantiate(portalPrefab, new Vector3(0,0,0), Quaternion.identity);
    }


    public int getFloor() {
        return floor;
    }

    public int getEnemiesKilled() {
        return enemiesKilled;
    }
}
