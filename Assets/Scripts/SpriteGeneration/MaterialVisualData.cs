using UnityEngine;

[CreateAssetMenu(fileName = "MaterialVisualData", menuName = "Scriptable Objects/MaterialVisualData")]
public class MaterialVisualData : ScriptableObject
{
    public string materialId;
    public string materialTag;
    public Color tintColor;
    public Sprite overrideSprite;
}
