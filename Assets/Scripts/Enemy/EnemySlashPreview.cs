using System.Collections;
using UnityEngine;

public class EnemySlashPreview : MonoBehaviour
{
    public float previewTime;
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf() {
        yield return new WaitForSeconds(previewTime);
        Destroy(gameObject);
    }

}
