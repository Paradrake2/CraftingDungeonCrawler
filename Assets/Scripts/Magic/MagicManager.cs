using UnityEngine;
using UnityEngine.SceneManagement;


public class MagicManager : MonoBehaviour
{
    public GameObject grid;
    public Transform gridParent;
    public GameObject createButton;
    public GameObject refreshButton;
    public SpellComponent selectedComponent;


    public void AddComponent()
    {
        // add selected component 
    }
    public void RefreshGridUI()
    {
        
    }

    public void OpenCraftingMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ReloadSpellGrid()
    {

    }
    public void CreateSpell()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
