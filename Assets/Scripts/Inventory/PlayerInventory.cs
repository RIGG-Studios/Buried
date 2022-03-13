using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//this class is the player inventory system, it will inherit from the database because it is an inventory system
public class PlayerInventory : ItemDatabase
{
    //reference to the player
    private Player player { get { return GetComponent<Player>(); } }

    //reference to the slot info
    [Header("Slots")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotGrid = null;

    //reference to the inventory info
    [Header("Inventory")]
    [SerializeField, Range(0, 10)] private int inventorySize;
    [SerializeField] private List<Item> startingItems = new List<Item>();

    public void Start()
    {
        //try to initialize the database as a new player inventory
        bool initialized = InitializeDatabase(startingItems.ToArray(), inventorySize, slotGrid, slotPrefab, true);

        //if it was succesfull, setup the item controllers that the starting items may or may not of contained
        if (initialized)
        {
            player.itemManagement.InitializeItemControllers(FindAllControllableItems());
        }

        //add input response to the slot #
        player.playerInput.Player.Slot1.performed += ctx => EnableSlot(0);
        //add input response to the slot #
        player.playerInput.Player.Slot2.performed += ctx => EnableSlot(1);
        //add input response to the slot #
        player.playerInput.Player.Slot3.performed += ctx => EnableSlot(2);
        //add input response to the slot #
        player.playerInput.Player.Slot4.performed += ctx => EnableSlot(3);
        //add input response to the slot #
        player.playerInput.Player.Slot5.performed += ctx => EnableSlot(4);
        //add input response to the slot #
        player.playerInput.Player.Slot5.performed += ctx => EnableSlot(5);

        //Find the UI from the canvas manager and update it to show on the screen.
        CanvasManager.instance.FindElementGroupByID("PlayerInventory").UpdateElements(0, 0, true);
    }

    //method used for enabling slots
    private void EnableSlot(int i)
    {
        //try to enable the that relates to i, if we are succesfull store the item the slot has in the Item var.
        Item item = null;
        bool success = TryEnableSlot(i, out item);
        //if we are succesfull and the output item is a tool, meaning it will have a controller
        if (success && item.item.controllable)
        {
            //set a new active controller in the item management class
            player.itemManagement.SetNewActiveController(item.item.itemType);
        }

        if(success && !item.item.controllable && item.item.activateType == ItemProperties.ActivationTypes.OnSlotSelected)
        {
            UseItem(item.item);
        }
    }

    public void DeselectSlot(Item item)
    {
        Slot slot = slotManager.FindSlotByItemProperties(item.item);

        if (slot != null && slot.selected)
        {
            player.itemManagement.SetNewActiveController(null);
            slot.SetColor(slot.dColor, false);
        }
    }

    private void OnEnable()
    {
        //subscribe to the UseItem event
        GameEvents.OnPlayerUseItem += UseItem;
    }

    private void OnDisable()
    {
        //unsubscribe to the UseItem event
        GameEvents.OnPlayerUseItem -= UseItem;
    }

    //Method used for using items from UI buttons.
    public void UseItem(ItemProperties item)
    {
        //if the item we are trying to use is a battery, meaning we are trying to update the flashlight battery
        if (item.itemType == ItemProperties.ItemTypes.Battery) 
        {
            if (!HasItem(ItemProperties.ItemTypes.Flashlight))
                return;

            //find the flashlight item controller
            ItemController controller = player.itemManagement.FindItemController(FindItem(ItemProperties.ItemTypes.Flashlight).item);

            if (controller != null)
            {
                //cast it to the flashlight controller class
                FlashlightController flashLight = (FlashlightController)controller;

                //set a new battery 
                flashLight.SetNewBattery(new Battery(100f, 5));

                //remove the item from the list as we just used one of them.
                RemoveItem(item, 1);
            }
        }
        else if(item.itemType == ItemProperties.ItemTypes.GrapplingHookAmmo)
        {
            if (!HasItem(ItemProperties.ItemTypes.GrapplingHook))
                return;

            RemoveItem(item, 1);
        }
    }

    //Method used for adding new items from chests for example
    public void AddNewItem(Item item, ChestInventory chest)
    {
        //add the item we selected to our inventory
        AddItem(item.item, item.stack);
        //remove it from the chest as we took it to our inventory
        chest.RemoveItemFromChest(item);
    }

    //override the base AddItem method as the player as some stuff to do when we add items.
    public override Item AddItem(ItemProperties itemProperties, int amount)
    {
        Item addItem = base.AddItem(itemProperties, amount);
        
        //check if the database succesfully added the item to its list, and then check if the item we are adding is a tool
        if (addItem != null && itemProperties.controllable)
        {
            //if so, setup a new item in the item management
            player.itemManagement.OnNewItemAdded(addItem);
        }

        //return the added item
        return addItem;
    }

    //override the base RemoveItem method as the player as some stuff to do when we add items.
    public override bool RemoveItem(ItemProperties itemProperties, int amount)
    {
        bool removeItem = base.RemoveItem(itemProperties, amount);

        //check if the database succesfully added the item to its list, and then check if the item we are adding is a tool
        if (removeItem && itemProperties.controllable)
        {           
            //if so, remove the item from the item management class
            player.itemManagement.OnItemRemoved(itemProperties);
        }

        return removeItem;
    }
}
