using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : ItemDatabase
{
    private Player player { get { return GetComponent<Player>(); } }

    [Header("Slots")]
    [SerializeField] private Transform slotGrid = null;
    [SerializeField] private GameObject slotPrefab;

    [Header("Inventory")]
    [SerializeField, Range(0, 10)] private int inventorySize;
    [SerializeField] private List<Item> startingItems = new List<Item>();

    public void Start()
    {
        bool initialized = InitializeDatabase(startingItems.ToArray(), inventorySize, slotGrid, slotPrefab, true);

        if (initialized)
        {
            player.itemManagement.SetupItemControllers(FindAllTools());
        }
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerUseItem += UseItem;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerUseItem -= UseItem;
    }

    public void UseItem(ItemProperties item)
    {
        if (item.consumableType == ItemProperties.ConsumableTypes.Battery) 
        {
            ItemController controller = player.itemManagement.FindItemController(FindItem(ItemProperties.WeaponTypes.Flashlight));
            FlashlightController flashLight = (FlashlightController)controller;

            flashLight.SetNewBattery(new Battery(100f, 5));

            RemoveItem(item, 1);
        }
        else if(item.toolType == ItemProperties.WeaponTypes.Flashlight)
        {
            player.itemManagement.ToggleItem(FindItem(ItemProperties.WeaponTypes.Flashlight));
        }
    }

    public void AddNewItem(Item item, ChestInventory chest)
    {
        AddItem(item.item, item.stack);
        chest.RemoveItemFromChest(item);
    }

    public override bool AddItem(ItemProperties itemProperties, int amount)
    {
        bool addItem = base.AddItem(itemProperties, amount);

        if(addItem && itemProperties.itemType == ItemProperties.ItemTypes.Tool)
            player.itemManagement.SetupNewItem(itemProperties);

        return addItem;
    }

    public override bool RemoveItem(ItemProperties itemProperties, int amount)
    {
        bool removeItem = base.RemoveItem(itemProperties, amount);

        if (removeItem && itemProperties.itemType == ItemProperties.ItemTypes.Tool)
            player.itemManagement.RemoveItem(itemProperties);

        return removeItem;
    }
}
