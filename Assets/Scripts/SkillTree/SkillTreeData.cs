using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTreeData", menuName = "Scriptable Objects/SkillTreeData")]
public class SkillTreeData : ScriptableObject
{
    public List<SkillNode> skills = new();
}
