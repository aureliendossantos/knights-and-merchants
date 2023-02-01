using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public List<InventoryEntry> contents = new List<InventoryEntry>();
    public void AddResource(Resource resource, int quantity)
    {
        int index = contents.FindIndex(entry => entry.resource == resource);
        if (index == -1)
        {
            Debug.LogWarning("This inventory cannot store this resource.");
            return;
        }
        contents[index].quantity += quantity;
    }
}

[Serializable]
public class InventoryEntry
{
    public Resource resource;
    public int quantity;
}
