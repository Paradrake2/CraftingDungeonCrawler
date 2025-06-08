using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SkillTreeUI : MonoBehaviour
{
    public Transform skillTreeParent;

    public GameObject displayUnlockedRecipesPrefab;
    public TextMeshProUGUI unlockedRecipesText;

    public GameObject displayAddedStatsPrefab;
    public TextMeshProUGUI addedStatsText;
    public static SkillTreeUI Instance;

    public PlayerStats playerStats;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        displayAddedStatsPrefab.SetActive(false);
        displayUnlockedRecipesPrefab.SetActive(false);
    }
    public void RefreshUI()
    {
        foreach (SkillButtonController controller in FindObjectsByType<SkillButtonController>(FindObjectsSortMode.None))
        {
            controller.RefreshState();
        }
    }
    public void BringUpUnlockedRecipes(CraftingRecipe[] recipes)
    {
        unlockedRecipesText.text = "";
        foreach (CraftingRecipe recipe in recipes)
        {
            unlockedRecipesText.text += "Unlocked " + recipe.recipeName + ". \n";
        }
        StartCoroutine(ShowUnlockedRecipes());
    }
    private IEnumerator ShowUnlockedRecipes()
    {
        displayUnlockedRecipesPrefab.SetActive(true);
        yield return new WaitForSeconds(3f);
        displayUnlockedRecipesPrefab.SetActive(false);
    }

    public void ShowAddedStats()
    {
        addedStatsText.text = "";
        foreach (var stat in playerStats.addedStats)
        {
            addedStatsText.text += stat + " \n";
        }
        StartCoroutine(ShowAddedStatsRoutine());
    }
    private IEnumerator ShowAddedStatsRoutine()
    {
        displayAddedStatsPrefab.SetActive(true);
        yield return new WaitForSeconds(2f);
        displayAddedStatsPrefab.SetActive(false);
    }
    public void loadEquipment()
    {
        SceneManager.LoadScene("EquipmentMenu");
    }
}
