using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadDungeon : MonoBehaviour
{
    private Button button;
    public LoadDungeon Instance;
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(onClick);
    }
    public void onClick() {
        SceneManager.LoadScene("Dungeon");
    }

    public void loadDungeon() {
        SceneManager.LoadScene("Dungeon");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
