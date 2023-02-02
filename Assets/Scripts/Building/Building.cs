using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Building : ScriptableObject
{
    public Sprite icon;
    public Sprite sprite;
    public Vector2Int spriteOffset;
    [TextArea(5, 5)]
    public string dimensions;
    public int entranceX;
    public AnimatorOverrideController animator;
    public AudioClip animationSound;
    public List<Sprite> productionSprites;
    public List<Vector2Int> productionOffsets;
    public List<Sprite> stockSprites;
    public List<Vector2Int> stockOffsets;
    [Header("Production")]
    public Resource production;
    public int productionQuantity;
}
