using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<ItemProperties> startingTools = new List<ItemProperties>();
    [SerializeField] private GameObject itemUI = null;
    [SerializeField] private Transform stackableParent = null;
    [SerializeField] private Transform itemParent = null;

    public bool initialized { get; private set; }
    public List<ItemController> tools { get; private set; }
    public List<Item> items { get; private set; }

    private Player player = null;

    private void Awake()
    {
        tools = new List<ItemController>();
        items = new List<Item>();

        player = GetComponent<Player>();
    }

    public void InitializeInventory()
    {
        if (initialized)
            return;

        if(startingTools.Count > 0)
        {
            for(int i = 0; i < startingTools.Count; i++)
            {
                ItemController controller = Instantiate(startingTools[i].itemPrefab, itemParent).GetComponent<ItemController>();

                controller.SetupController(player, startingTools[i]);
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.OnStartGame += StartGame;
    }

    private void OnDisable()
    {
        GameEvents.OnStartGame -= StartGame;
    }

    public void StartGame()
    {
        CanvasManager.instance.FindElementGroupByID("PlayerInventory").UpdateElements(0, 0, true);
    }

    private void Update()
    {
        float scroll = player.playerInput.Player.ScrollUp.ReadValue<float>();

        if(scroll > 0)
        {
            Debug.Log("ya");
        }
        else if(scroll < 0)
        {
            Debug.Log("down");
        }
    }

    public bool AddItem(ItemProperties properties, int amount)
    {
        switch (properties.controllable)
        {
            case true:
                return AddControllableItem(properties);

            case false:
                return AddStackableItem(properties, amount);
        }
    }

    private bool AddControllableItem(ItemProperties properties)
    {
        ItemController controller = Instantiate(properties.itemPrefab, itemParent).GetComponent<ItemController>();

        controller.SetupController(player, properties);
        tools.Add(controller);

        return true;
    }

    private bool AddStackableItem(ItemProperties properties, int amount)
    {
        bool success = false;
        Item item = null;
        HasItem(properties.itemType, out item);

        if(item != null)
        {
            int nextStack = item.stack + amount;

            if (nextStack <= properties.stackAmount)
            {
                item.stack += amount;
                success = true;
            }
            else if (nextStack <= 0)
                success = false;
        }
        else
        {
            success = SpawnNewStackableItem(properties, amount);
        }

        return success;
    }

    private bool SpawnNewStackableItem(ItemProperties properties, int amt)
    {
        bool success = false;
        Item item = new Item(properties, amt);
        Slot slot = Instantiate(itemUI, stackableParent).GetComponent<Slot>();

        if(slot != null)
        {
            slot.SetupSlot(item);
            success = true;
        }

        items.Add(item);
        return success;
    }

    public bool RemoveItem(ItemProperties properties, int amount)
    {
        bool success = false;

        if (properties.controllable)
            success = false;

        AddStackableItem(properties, -amount);

        return success;
    }

    public void HasItem(ItemProperties.ItemTypes type, out Item itm)
    {
        itm = null;

        for(int i = 0; i < items.Count; i++)
        {
            if(type == items[i].item.itemType)
            {
                itm = items[i];
                break;
            }
        }
    }
}

[System.Serializable]
public class Item
{
    public ItemProperties item = null;
    public int stack = 0;

    public Item(ItemProperties item, int stack)
    {
        this.item = item;
        this.stack = stack;
    }
}
