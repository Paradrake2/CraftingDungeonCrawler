using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Scriptable Objects/Augment")]
public class Augment : ScriptableObject
{
    public string augmentName;
    public string description;
    public Sprite icon;
    public List<StatModifier> modifiers;
    
}
