using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    public RoomGenerator roomGenerator;
    public GameObject playerPrefab;
    public GameObject newFloorOpen;
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
    void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        newFloorOpen.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Dungeon")
        {
            newFloorOpen = GameObject.Find("PortalOpen");
            floor++;
            roomGenerator = FindFirstObjectByType<RoomGenerator>();
            if (roomGenerator != null)
            {
                roomGenerator.ClearRoom();
                roomGenerator.GenerateRoom();
            }
            else
            {
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
        if (enemiesKilled >= totalEnemies / 3){
            DisplayPortalLoaded();
            Debug.Log("1/3 of enemies killed");
            SpawnPortal();
        }
    }
    void DisplayPortalLoaded()
    {
        newFloorOpen.SetActive(true);
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
