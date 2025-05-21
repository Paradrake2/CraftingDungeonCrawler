using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
public class Log : MonoBehaviour
{
    public Transform log;
    public GameObject lootLogMessage;
    public InventorySystem inventorySystem;
    public static Log Instance;
    public float entryDuration = 3f;
    public int maxEntries = 5;

    private Queue<GameObject> logEntries = new Queue<GameObject>();
    void Start()
    {
        inventorySystem = FindFirstObjectByType<InventorySystem>();
    }

    public void AddLogMessage(string itemId, int amount)
    {
        GameObject newLogEntry = Instantiate(lootLogMessage, log);
        TextMeshProUGUI logText = newLogEntry.GetComponent<TextMeshProUGUI>();
        logText.text = $"Acquired {itemId} x{amount}";
        logEntries.Enqueue(newLogEntry);
        if (logEntries.Count > maxEntries)
        {
            GameObject oldEntry = logEntries.Dequeue();
            Destroy(oldEntry);
        }


        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)log);
        StartCoroutine(RemoveLogEntry(newLogEntry, entryDuration));
    }

    private IEnumerator RemoveLogEntry(GameObject logEntry, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (logEntries.Contains(logEntry))
        {
            logEntries = new Queue<GameObject>(logEntries.Where(e => e != logEntry));
            Destroy(logEntry);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
