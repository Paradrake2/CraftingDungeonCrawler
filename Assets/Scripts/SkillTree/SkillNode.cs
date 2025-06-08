using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[System.Serializable]
public class SerializableStat
{
    public StatType statType;
    public float value;
}
[System.Serializable]
public class SkillNode
{
    public string nodeID;
    public string nodeType;
    public string description;
    public Sprite icon;
    public List<string> prerequisites = new List<string>();
    public bool unlocked = false;
    public CraftingRecipe[] recipeUnlocks;
    public SerializableStat[] addedStats;

    public bool acquired = false;

    public void SetAcquired(bool acquiredBool)
    {
        acquired = acquiredBool;
    }
}
