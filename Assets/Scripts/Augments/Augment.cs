using System.Collections.Generic;
using UnityEngine;

public enum AugmentRarity {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythical
}
[CreateAssetMenu(fileName = "Augment", menuName = "Scriptable Objects/Augment")]
public class Augment : ScriptableObject
{
    public string augmentName;
    public string augmentId;
    public string description;
    public Sprite icon;
    public List<StatModifier> modifiers;
    public float dropRate;
    public AugmentRarity augmentRarity;
    public int dropFloor;
}
