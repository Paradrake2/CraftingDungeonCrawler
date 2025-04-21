using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    public int bossEveryXFloors = 5;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
            Debug.Log("Player entered portal. Loading next floor.");
            DungeonManager.Instance.floor++;
            if (DungeonManager.Instance.floor % bossEveryXFloors == 0) {
                // Go to boss scene
                int bossIndex = DungeonManager.Instance.floor / bossEveryXFloors;
                string bossSceneName = "BossScene " + bossIndex;
                SceneManager.LoadScene(bossSceneName);
            } else {
                // Regular floor
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
