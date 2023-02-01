using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    public Building building;
    Image image;
    [SerializeField] GridState grid;
    [Header("Temp: override for Road")]
    public bool road;
    [SerializeField] Sprite roadSprite;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = road ? roadSprite : building.icon;
    }

    public void OnClick()
    {
        if (road)
        {
            grid.ChangeState(grid.placingTileState);
            return;
        }
        grid.building = building;
        grid.ChangeState(grid.placingBuildingState);
    }
}
