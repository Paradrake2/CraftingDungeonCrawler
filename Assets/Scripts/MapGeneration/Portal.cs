using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    public int bossEveryXFloors = 5;
    private bool isActivated = false;
    public static bool isSceneLoading = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSceneLoading)
        {
            isSceneLoading = true;
            StartCoroutine(LoadNextFloor());
        }
    }
    private IEnumerator LoadNextFloor()
    {
        Debug.LogError("CALLED LOAD NEXT FLOOR");
        yield return new WaitForSeconds(0.2f);
        isSceneLoading = false;
        SceneManager.LoadScene("Dungeon");
    }
}
