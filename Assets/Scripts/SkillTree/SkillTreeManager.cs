using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;
    public SkillTreeData node;
    public Dictionary<string, SkillNode> skillLookup = new();
    public PlayerStats playerStats;
    void Awake()
    {
        if (playerStats == null) playerStats = FindFirstObjectByType<PlayerStats>();
        if (Instance == null)
        {
            Instance = this;
            InitializeSkillSetup();
        }
        
    }
    void InitializeSkillSetup()
    {
        foreach (SkillNode skill in node.skills)
        {
            SkillNode copy = new SkillNode
            {
                nodeID = skill.nodeID,
                icon = skill.icon,
                unlocked = skill.unlocked,
                prerequisites = new List<string>(skill.prerequisites),
                addedStats = skill.addedStats,
                recipeUnlocks = skill.recipeUnlocks,
                acquired = CheckIfAcquired(skill.nodeID)
            };
            skillLookup[skill.nodeID] = copy;
        }
    }
    bool CheckIfAcquired(string skillID)
    {
        return playerStats.acquiredSkillIDs.Contains(skillID);
    }
    public bool Acquired(string skillID)
    {
        SkillNode thisSkill;
        if (skillLookup.ContainsKey(skillID))
        {
            thisSkill = skillLookup[skillID];
            if (thisSkill.acquired) return true;
            return false;
        }
        return false;
    }
    public bool CanUnlock(string skillID)
    {
        Debug.Log("Is in skillLookup: " + skillLookup.ContainsKey(skillID));
        if (!skillLookup.ContainsKey(skillID)) return false;
        SkillNode skill = skillLookup[skillID];
        Debug.Log($"{skillID}Is skill unlocked: " + skill.unlocked);
        //if (!skill.unlocked) return false;

        foreach (string prereq in skill.prerequisites)
        {
            Debug.Log("Next: " + (skillLookup.ContainsKey(prereq) || !skillLookup[prereq].unlocked));
            if (!skillLookup.ContainsKey(prereq) || !skillLookup[prereq].acquired) return false;
        }
        return playerStats.skillPoints > 0;
    }

    public void UnlockSkill(string skillID)
    {
        Debug.Log("Calling unlock skill");
        if (!CanUnlock(skillID)) return;

        
        playerStats.skillPoints--;
        SkillNode thisNode = skillLookup[skillID];
        thisNode.unlocked = true;
        thisNode.acquired = true;
        // Apply skill effects here
        if (thisNode.recipeUnlocks != null)
        {
            foreach (CraftingRecipe recipe in thisNode.recipeUnlocks)
            {
                UnlockRecipes(recipe);
            }
            SkillTreeUI.Instance.BringUpUnlockedRecipes(thisNode.recipeUnlocks);
        }

        if (thisNode.addedStats != null)
        {
            foreach (var stat in thisNode.addedStats)
            {
                AddSkillStats(stat.statType, stat.value);
            }
            SkillTreeUI.Instance.ShowAddedStats();
            playerStats.AddBonusStats();
        }
        playerStats.acquiredSkillIDs.Add(skillID);
        SkillTreeUI.Instance.RefreshUI();
    }

    public void UnlockRecipes(CraftingRecipe recipe)
    {
        PlayerRecipeBook.Instance.LearnRecipe(recipe);
    }
    public void AddSkillStats(StatType stat, float amount)
    {
        playerStats.addedStats.Add(stat, amount);
    }

    void Start()
    {
        if (playerStats == null) playerStats = FindFirstObjectByType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
