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
    public Animation idle;
    public Vector2 idleOffset;
    [Header("Production")]
    public Resource production;
    public int productionQuantity;
}
