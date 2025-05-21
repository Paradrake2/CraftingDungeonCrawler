using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Transform menu;
    public void PressMenuButton()
    {
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    /*
    public void OpenSettings() {
        // either load scene Settings(once created) or open settings menu(prefab)
    }
    */

    void Start()
    {
        menu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
