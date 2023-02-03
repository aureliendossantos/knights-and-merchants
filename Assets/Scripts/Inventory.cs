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
        if (contents[index].IsFull())
        {
            Debug.LogWarning("Inventory reached its maximum.");
            return;
        }
        contents[index].quantity += quantity;
    }
    public void RemoveResource(Resource resource, int quantity)
    {
        int index = contents.FindIndex(entry => entry.resource == resource);
        if (index == -1)
        {
            Debug.LogWarning("This inventory cannot remove this resource.");
            return;
        }
        if (contents[index].quantity <= 0)
        {
            Debug.LogWarning("Inventory empty.");
            return;
        }
        contents[index].quantity -= quantity;
    }
}

[Serializable]
public class InventoryEntry
{
    public Resource resource;
    public int quantity;
    public int? maximum;

    public bool IsFull()
    {
        return maximum.HasValue && quantity >= maximum;
    }

    public InventoryEntry(Resource resource, int? maximum = null, int quantity = 0)
    {
        this.resource = resource;
        this.maximum = maximum;
        this.quantity = quantity;
    }
}
