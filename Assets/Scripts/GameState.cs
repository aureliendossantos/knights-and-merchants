using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] InventoryPanel inventoryPanel;

    void Start()
    {
        inventoryPanel.SetParameters(inventory);
    }
}
