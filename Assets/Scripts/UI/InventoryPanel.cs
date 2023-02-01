using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Helpers;

public class InventoryPanel : MonoBehaviour
{
    List<Resource> resourcesToDisplay;
    Inventory inventory;
    [SerializeField] ResourcePanel resourcePanelPrefab;
    List<ResourcePanel> resourcesPanels = new List<ResourcePanel>();

    void Awake()
    {
        resourcesToDisplay = Resources.LoadAll<Resource>("").ToList();
        Debug.Log(resourcesToDisplay.Count + " Resources Loaded");
    }

    public void SetParameters(Inventory inventory)
    {
        this.inventory = inventory;
        DestroyChildren(gameObject);
        resourcesPanels = new List<ResourcePanel>();
        foreach (var resource in resourcesToDisplay)
        {
            var currentPanel = Instantiate(resourcePanelPrefab, transform);
            currentPanel.resource = resource;
            currentPanel.icon.sprite = resource.icon;
            resourcesPanels.Add(currentPanel);
        }
        Debug.Log("Set up " + resourcesPanels.Count + "Inventory Panels");
    }

    void Update()
    {
        if (inventory == null) return;
        foreach (var entry in inventory.contents)
        {
            var currentPanel = resourcesPanels.Find(panel => panel.resource == entry.resource);
            if (currentPanel != null)
            {
                currentPanel.SetQuantity(entry.quantity);
            }
            else { Debug.LogWarning("Inventory Panel not found"); };
        }
    }
}
