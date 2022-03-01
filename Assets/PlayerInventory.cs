using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : ItemDatabase
{
    [SerializeField] private Transform slotGrid = null;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField, Range(0, 10)] private int inventorySize;
    [SerializeField] private List<Item> startingItems = new List<Item>();

    public void Start()
    {
        InitializeDatabase(startingItems.ToArray(), inventorySize, slotGrid, slotPrefab);
    }


    public override bool AddItem(ItemProperties itemProperties, int amount)
    {
        return base.AddItem(itemProperties, amount);
    }

    public override bool RemoveItem(ItemProperties itemProperties, int amount)
    {
        return base.RemoveItem(itemProperties, amount);
    }
}
