using System.Collections;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float destroyTime;
    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
