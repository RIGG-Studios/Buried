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

        player.playerInput.Player.Slot1.performed += ctx => EnableSlot(0);
        player.playerInput.Player.Slot2.performed += ctx => EnableSlot(1);
        player.playerInput.Player.Slot3.performed += ctx => EnableSlot(2);
        player.playerInput.Player.Slot4.performed += ctx => EnableSlot(3);
        player.playerInput.Player.Slot5.performed += ctx => EnableSlot(4);
        player.playerInput.Player.Slot5.performed += ctx => EnableSlot(5);

        CanvasManager.instance.FindElementGroupByID("PlayerInventory").UpdateElements(0, 0, true);
    }

    private void EnableSlot(int i)
    {
        Item item = null;
        bool success = TryEnableSlot(i, out item);

        if (success && item.item.itemType == ItemProperties.ItemTypes.Tool)
        {
            player.itemManagement.SetNewItemController(item.item);
            item.slot.SetColor(item.slot.hColor);
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
            player.itemManagement.UseItem(FindItem(ItemProperties.WeaponTypes.Flashlight));
        }
    }

    public void AddNewItem(Item item, ChestInventory chest)
    {
        AddItem(item.item, item.stack);
        chest.RemoveItemFromChest(item);
    }

    public override Item AddItem(ItemProperties itemProperties, int amount)
    {
        Item addItem = base.AddItem(itemProperties, amount);
        if (addItem != null && itemProperties.itemType == ItemProperties.ItemTypes.Tool)
        {
            player.itemManagement.SetupNewItem(addItem);
        }

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
