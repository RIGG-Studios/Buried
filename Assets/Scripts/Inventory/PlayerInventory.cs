using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//this class is the player inventory system, it will inherit from the database because it is an inventory system
public class PlayerInventory : ItemDatabase
{
    //reference to the player
    private Player player { get { return GetComponent<Player>(); } }

    //reference to the slot info
    [Header("Slots")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Slot currentItemSlot = null;
    [SerializeField] private Transform slotGrid = null;

    //reference to the inventory info
    [Header("Inventory")]
    [SerializeField, Range(0, 10)] private int inventorySize;
    [SerializeField] private Transform itemParent;
    [SerializeField] private List<Item> startingItems = new List<Item>();

    private List<ItemController> controllableItems = new List<ItemController>();
    private int currentItemIndex = 0;
    public ItemController currentControllableItem { get; private set; }

    public TextMeshProUGUI notesCollected;

    public void Start()
    {
        //try to initialize the database as a new player inventory
        bool initialized = InitializeDatabase(startingItems.ToArray(), inventorySize, slotGrid, slotPrefab, true);

        //if it was succesfull, setup the item controllers that the starting items may or may not of contained
        if (initialized)
        {
            Item[] props = startingItems.ToArray();

            for (int i = 0; i < props.Length; i++)
            {
                ItemController obj = Instantiate(props[i].item.itemPrefab, itemParent).GetComponent<ItemController>();

                if (obj != null)
                {
                    obj.transform.localPosition = obj.startingPosition;
                    obj.transform.localRotation = Quaternion.Euler(obj.startingRotation);
                    obj.SetupController(player, props[i]);

                    controllableItems.Add(obj);
                }
            }
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

        player.playerInput.Player.Fire.performed += ctx => UseCurrentControllableItem();
        notesCollected.enabled = false;
        currentItemSlot.gameObject.SetActive(false);
        SwitchItems(0);
    }

    private void Update()
    {
        float scroll = player.playerInput.Player.ScrollUp.ReadValue<float>();

        if (scroll >= 1f)
        {
            SwitchItems(1);
        }
        else if (scroll <= -1f)
        {
            SwitchItems(-1);
        }

        notesCollected.text = ($"{MainManager.GetRemainingNotes(this)} notes remaining");
    }

    private void UseCurrentControllableItem()
    {
        if (currentControllableItem == null)
            return;

        currentControllableItem.UseItem();
    }

    private void SwitchItems(int i)
    {
        currentItemIndex += i;

        if (currentItemIndex > controllableItems.Count - 1)
            currentItemIndex = 0;
        else if (currentItemIndex < 0)
            currentItemIndex = controllableItems.Count - 1;

        ItemController nextController = controllableItems[currentItemIndex];

        if(nextController != null)
        {
            if (currentControllableItem != null)
            {
                currentItemSlot.ResetSlot();
                currentControllableItem.ResetItem();
            }

            currentControllableItem = nextController;
            nextController.ActivateItem();
            currentItemSlot.AddItem(currentControllableItem.itemInInventory);
        }
        else
        {
            currentItemSlot.ResetSlot();
        }
    }

    private void EnableSlot(int i)
    {
        Item item = null;
        bool success = TryEnableSlot(i, out item);

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
            slot.SetColor(slot.dColor, false);
        }
    }

    private void OnEnable()
    {
        //subscribe to the UseItem event
        GameEvents.OnPlayerUseItem += UseItem;
        GameEvents.OnStartGame += StartGame;
    }

    private void OnDisable()
    {
        //unsubscribe to the UseItem event
        GameEvents.OnPlayerUseItem -= UseItem;
        GameEvents.OnStartGame -= StartGame;
    }

    private void StartGame()
    {
        //Find the UI from the canvas manager and update it to show on the screen.
        CanvasManager.instance.FindElementGroupByID("PlayerInventory").UpdateElements(0, 0, true);
        currentItemSlot.gameObject.SetActive(true);
        notesCollected.enabled = true;
    }

    //Method used for using items from UI buttons.
    public void UseItem(ItemProperties item)
    {
        //if the item we are trying to use is a battery, meaning we are trying to update the flashlight battery
        if (item.itemType == ItemProperties.ItemTypes.Battery) 
        {
            //find the flashlight item controller
            ItemController controller = FindItemController(ItemProperties.ItemTypes.Flashlight);

            if (controller != null)
            {
                FlashlightController flashLight = (FlashlightController)controller;

                flashLight.SetNewBattery(new Battery(100f, 5));
                RemoveItem(item, 1);
            }
        }
        else if(item.itemType == ItemProperties.ItemTypes.GrapplingHookAmmo)
        {
            RemoveItem(item, 1);
        }
        else if(item.itemType == ItemProperties.ItemTypes.Flare)
        {
            ItemController flare = FindItemController(ItemProperties.ItemTypes.Flare);
            flare.itemInInventory.stack--;
            currentItemSlot.UpdateSlotStack(flare.itemInInventory.stack);
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

        if(addItem != null)
        {
            if (itemProperties.itemType == ItemProperties.ItemTypes.Note)
            {
                Item itm = FindItem(ItemProperties.ItemTypes.Note);

                if (itm != addItem)
                {
                    itm.stack++;
                    RemoveItem(itemProperties, 1);
                }
            }
            else if(itemProperties.itemType == ItemProperties.ItemTypes.Flare)
            {
                ItemController flare = FindItemController(ItemProperties.ItemTypes.Flare);
                flare.itemInInventory.stack++;
                currentItemSlot.UpdateSlotStack(flare.itemInInventory.stack);
                RemoveItem(itemProperties, 1);
            }
        }

        return addItem;
    }

    public override bool RemoveItem(ItemProperties itemProperties, int amount)
    {
        bool removeItem = base.RemoveItem(itemProperties, amount);

        return removeItem;
    }

    public ItemController FindItemController(ItemProperties.ItemTypes type)
    {
        for(int i = 0; i < controllableItems.Count; i++)
        {
            if (type == controllableItems[i].baseItem.itemType)
                return controllableItems[i];
        }

        return null;
    }
    public ItemController GetCurrentControllableItem()
    {
        return currentControllableItem;
    }
}
