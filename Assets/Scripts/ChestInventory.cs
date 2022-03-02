using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : ItemDatabase
{
    [Header("Slots")]
    [SerializeField] private Transform slotGrid = null;
    [SerializeField] private GameObject slotPrefab = null;

    [Header("Inventory")]
    [SerializeField, Range(0, 10)] private int inventorySize = 4;
    [SerializeField] private List<Item> itemsInChest = new List<Item>();


    public void OnInteract()
    {
        if (initialized)
            ResetDatabase();

        InitializeDatabase(itemsInChest.ToArray(), inventorySize, slotGrid, slotPrefab, false);
    }

    public void OnStopInteract()
    {
        ResetDatabase();
    }

    public void RemoveItemFromChest(Item item)
    {
        RemoveItem(item.item, item.stack);
        itemsInChest.Remove(FindItemInChest(item.item));
    }

    private Item FindItemInChest(ItemProperties props)
    {
        for(int i = 0; i < itemsInChest.Count; i++)
        {
            if (props == itemsInChest[i].item)
                return itemsInChest[i];
        }

        return null;
    }
}
