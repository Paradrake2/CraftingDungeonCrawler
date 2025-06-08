using UnityEngine;
using UnityEngine.UI;
public class SpellGridSlot : MonoBehaviour
{
    public Image overlay;
    public Image icon;
    public Button button;

    public int gridX;
    public int gridY;
    public bool isUnlocked;

    public SpellComponent heldComponent;

    public void RemoveComponent()
    {
        // remove component from grid slot, return it to inventory
    }

    public void Initialize(int x, int y, bool unlocked)
    {
        gridX = x;
        gridY = y;
        isUnlocked = unlocked;

        if (overlay != null) overlay.gameObject.SetActive(!unlocked);

        GetComponent<Button>().interactable = unlocked;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
