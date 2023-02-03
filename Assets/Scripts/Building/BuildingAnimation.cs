using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAnimation : MonoBehaviour
{
    BuildingState buildingState;

    void Start()
    {
        buildingState = GetComponentInParent<BuildingState>();
    }

    public void UpdateStockSprite()
    {
        buildingState.UpdateStockSprite();
    }

    public void UpdateProductionSprite()
    {
        buildingState.UpdateProductionSprite();
    }
}
