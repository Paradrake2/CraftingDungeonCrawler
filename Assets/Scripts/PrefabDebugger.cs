using UnityEngine;

public class PrefabDebugger : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
        var instance = Instantiate(prefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
