using System.Collections.Generic;
using UnityEngine;
public enum SpellComponentType
{
    Projectile,
    Velocity,
    StatusEffect,
    TimeDelay,
    AreaOfEffect,
    ManaCost,
    Piercing,
    ProjectileSize,
}

public enum StatusEffectType
{
    None,
    Burn,
    Freeze,
    Poison,
    Stun,
    Slow,
    Weakness,
    DefenseBreak,
    Marked
}

[CreateAssetMenu(fileName = "SpellComponent", menuName = "Scriptable Objects/SpellComponent")]

public class SpellComponent : ScriptableObject
{
    public SpellComponentType type;
    public Sprite icon;
    public string componentName;

    [Header("Generic Values")]
    public Sprite projectileSprite;
    public float floatValue;
    public int intValue;
    public bool boolValue;
    public StatusEffectType statusEffect; // only used if type is StatusEffect
    public Color spellColor;

}
