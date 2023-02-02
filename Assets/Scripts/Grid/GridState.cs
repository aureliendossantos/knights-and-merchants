using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using static Helpers;

public class GridState : StateMachine, IDragHandler, IPointerDownHandler, IPointerMoveHandler, IPointerExitHandler
{
    // Declaring states
    public Idle idleState;
    public PlacingTile placingTileState;
    public PlacingBuilding placingBuildingState;

    [SerializeField] GameState gameState;
    [SerializeField] AudioManager audioManager;
    [SerializeField] RuleTile pathTile;
    [SerializeField] Tile defaultTile;
    public Building building;
    [SerializeField] BuildingState buildingPrefab;
    bool buildingPossible;
    bool placingRoad;

    [Header("UI Tiles")]
    [SerializeField] Tile highlightTile;
    [SerializeField] Tile wrongTile;
    [SerializeField] Tile entranceTile;
    [SerializeField] Tile entranceWrongTile;

    Grid grid;
    [Header("Tilemaps")]
    [SerializeField] Tilemap groundLayer;
    [SerializeField] Tilemap roadLayer;
    [SerializeField] Tilemap buildingsLayer;
    public Tilemap uiLayer;

    Vector3Int mousePos;
    Vector3Int previousMousePos = new Vector3Int();

    /// <summary>
    /// StateMachine boilerplate: create states
    /// </summary>
    void Awake()
    {
        idleState = new Idle(this);
        placingTileState = new PlacingTile(this);
        placingBuildingState = new PlacingBuilding(this);
        grid = gameObject.GetComponent<Grid>();
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiLayer.ClearAllTiles();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        mousePos = GetMousePosition();
        if (currentState == placingTileState)
        {
            // Highlight tile
            if (!mousePos.Equals(previousMousePos))
            {
                uiLayer.SetTile(previousMousePos, null); // Remove old highlightTile
                uiLayer.SetTile(mousePos, highlightTile);
                previousMousePos = mousePos;
            }
        }
        if (currentState == placingBuildingState)
        {
            buildingPossible = true;
            uiLayer.ClearAllTiles();
            using (StringReader reader = new StringReader(building.dimensions))
            {
                string line;
                int y = 3;
                while ((line = reader.ReadLine()) != null)
                {
                    foreach ((char c, int index) in line.WithIndex())
                    {
                        int x = index - building.entranceX;
                        Vector3Int position = mousePos + new Vector3Int(x, y, 0);
                        if (char.GetNumericValue(c) == 1)
                        {
                            bool validCell = NearbyTilesAreClear(position);
                            uiLayer.SetTile(position, validCell ? highlightTile : wrongTile);
                            if (x == 0 && y == 0)
                                uiLayer.SetTile(position, validCell ? entranceTile : entranceWrongTile);
                            if (!validCell) buildingPossible = false;
                        }
                    }
                    y -= 1;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == placingTileState)
            if (eventData.button == PointerEventData.InputButton.Left)
                TryToPlaceRoad();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ChangeState(idleState);
            return;
        }
        if (currentState == placingTileState)
            if (eventData.button == PointerEventData.InputButton.Left)
                TryToPlaceRoad();
        if (currentState == placingBuildingState && buildingPossible)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Vector3Int pivotCell = new Vector3Int();
                bool pivotSet = false;
                int width = 0;
                using (StringReader reader = new StringReader(building.dimensions))
                {
                    string line;
                    int y = 3;
                    while ((line = reader.ReadLine()) != null)
                    {
                        width = 0;
                        foreach ((char c, int index) in line.WithIndex())
                        {
                            int x = index - building.entranceX;
                            if (char.GetNumericValue(c) == 1)
                            {
                                if (!pivotSet)
                                {
                                    pivotCell = mousePos + new Vector3Int(x, y + 1, 0);
                                    pivotSet = true;
                                }
                                buildingsLayer.SetTile(mousePos + new Vector3Int(x, y, 0), highlightTile);
                                width += 1;
                            }
                            if (x == 0 && y == 0)
                                AddRoadTile(mousePos + new Vector3Int(x, y, 0));
                        }
                        y -= 1;
                    }
                }
                // Debug.Log(grid.CellToWorld(grid.WorldToCell(mousePos)));
                audioManager.Build();
                Debug.Log(width);
                // Placement with txt values
                Vector3 buildingOffset = new Vector3(building.spriteOffset.x / 40f, -building.spriteOffset.y / 40f);
                Instantiate(
                    buildingPrefab,
                    grid.CellToWorld(
                        grid.WorldToCell(pivotCell)
                        + new Vector3Int(width == 4 ? 2 : 1, -1, 0)
                    )
                    + buildingOffset
                    ,
                    Quaternion.identity)
                    .SetParameters(building, grid.CellToWorld(mousePos + new Vector3Int(width == 4 ? 1 : 0, 1, 0)), gameState);
                // Manual placement
                // Instantiate(buildingPrefab, grid.CellToWorld(grid.WorldToCell(mousePos)) + new Vector3(0.5f, 0, 0), Quaternion.identity).SetParameters(building, gameState);
                ChangeState(idleState);
            }
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    void AddRoadTile(Vector3Int position)
    {
        roadLayer.SetTile(position, pathTile);
    }

    void RemoveRoadTile(Vector3Int position)
    {
        roadLayer.SetTile(position, null);
    }

    /// <summary>
    /// Checks if nearby tiles have buildings and displays the cross icon if so.
    /// </summary>
    /// <param name="position">Cell position</param>
    /// <returns>True if nearby tiles are clear</returns>
    bool NearbyTilesAreClear(Vector3Int position)
    {
        bool clear = true;
        foreach (var cell in nearbyCells)
        {
            Vector3Int nearbyPosition = position + new Vector3Int(cell.x, cell.y);
            if (buildingsLayer.HasTile(nearbyPosition))
            {
                uiLayer.SetTile(nearbyPosition, wrongTile);
                clear = false;
            };
        }
        return clear;
    }

    /// <summary>
    /// Places a road tile if there's no road nor bulding present.
    /// Todo: sound when wrong.
    /// </summary>
    void TryToPlaceRoad()
    {
        if (!roadLayer.HasTile(mousePos) && !buildingsLayer.HasTile(mousePos))
        {
            AddRoadTile(mousePos);
            audioManager.PlaceMarker();
        }
    }
}