using UnityEngine;

public class PanelToggleManager : MonoBehaviour
{
    public GameObject craftingPanel;
    public GameObject refiningPanel;

    public void showCrafting() {
        craftingPanel.SetActive(true);
        refiningPanel.SetActive(false);
    }

    public void ShowRefining() {
        craftingPanel.SetActive(false);
        craftingPanel.SetActive(true);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
