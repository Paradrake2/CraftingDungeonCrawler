using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CompiledSpell
{
    public string spellName;

    // functionality
    public SpellComponentType coreType;
    public List<StatusEffectType> statusEffects = new();
    public float manaCost;
    public float delayTime;
    public PlayerProjectile projectile;

    // visuals
    public Color tintColor;
    public Sprite projectileIcon;
}
