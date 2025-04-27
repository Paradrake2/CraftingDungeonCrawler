using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    public int totalEnemies = 0;
    public int enemiesKilled = 0;
    public int floor;

    public GameObject portalPrefab;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getFloor() {
        return floor;
    }

    public int getEnemiesKilled() {
        return enemiesKilled;
    }
}
