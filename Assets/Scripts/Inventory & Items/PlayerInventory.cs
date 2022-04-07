using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject itemUI = null;
    [SerializeField] private Transform itemParent = null;

    public bool initialized { get; private set; }
    public List<ItemController> tools { get; private set; }
    public List<Item> items { get; private set; }
    public ItemController currentTool { get; private set; }

    private Player player = null;
    private Transform stackableParent = null;

    private UIElementGroup inventoryGroup = null;
    private UIElement equipSlider = null;
    private ImageElement toolIcon = null;

    private int currentToolIndex = 0;
    private bool isEquipping = false;

    private void Awake()
    {
        tools = new List<ItemController>();
        items = new List<Item>();

        player = GetComponent<Player>();
    }

    private void Start()
    {
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        if (initialized)
            return;

        inventoryGroup = player.playerCanvas.FindElementGroupByID("PlayerInventory");

        if (inventoryGroup != null)
        {
            toolIcon = (ImageElement)inventoryGroup.FindElement("toolicon");
            equipSlider = inventoryGroup.FindElement("equipslider");
        }

        stackableParent = player.playerCanvas.FindElementGroupByID("PlayerInventory").FindElement("grid").transform;

        player.playerInput.Player.Fire.performed += ctx => UseTool();
        initialized = true;
    }

    private void OnEnable()
    {
        GameEvents.OnStartGame += StartGame;
    }

    private void OnDisable()
    {
        GameEvents.OnStartGame -= StartGame;
    }

    public void StartGame(LevelProperties properties)
    {
        if (!initialized)
        {
            InitializeInventory();
        }

        inventoryGroup.UpdateElements(0, 0, true);
        toolIcon.SetActive(false);

        LoadStartingItems(properties.startingTools.ToArray(), properties.startingItems.ToArray());
    }

    private void LoadStartingItems(ItemProperties[] startingTools, ItemProperties[] startingItems)
    {
        if (startingTools.Length > 0)
        {
            for (int i = 0; i < startingTools.Length; i++)
            {
                ItemController controller = Instantiate(startingTools[i].itemPrefab, itemParent).GetComponent<ItemController>();

                controller.SetupController(player, startingTools[i]);
                tools.Add(controller);
            }

            SwitchItems(0);
        }

        if (startingItems.Length > 0)
        {
            for (int i = 0; i < startingItems.Length; i++)
            {
                AddItem(startingItems[i], 1);
            }
        }
    }

    private void UseTool()
    {
        if (currentTool == null)
            return;

        currentTool.UseItem();
    }

    public void UseItem(ItemProperties.ItemTypes type)
    {
        Item item = null;
        HasItem(type, out item);

        if(item != null)
        {
            RemoveItem(item.item, 1);
        }
    }

    private void Update()
    {
        float scroll = player.playerInput.Player.Scroll.ReadValue<float>();

        if(scroll > 0)
        {
            SwitchItems(1);
        }
        else if(scroll < 0)
        {
            SwitchItems(-1);
        }
    }

    private void SwitchItems(int i)
    {
        if (isEquipping || tools.Count <= 0)
            return;

        toolIcon.SetActive(true);
        currentToolIndex += i;

        if (currentToolIndex > tools.Count - 1)
            currentToolIndex = 0;
        else if (currentToolIndex < 0)
            currentToolIndex = tools.Count - 1;

        StartCoroutine(SwitchTools(tools[currentToolIndex]));
    }

    public bool AddItem(ItemProperties properties, int amount)
    {
        if (properties.stackable)
        {
            return AddStackableItem(properties, amount);
        }

        if (properties.controllable)
        {
            return AddControllableItem(properties);
        }

        return false;
    }

    private bool AddControllableItem(ItemProperties properties)
    {
        if (FindTool(properties))
            return false;

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
            int nextStack = (item.stack+ amount);

            if (nextStack <= properties.stackAmount)
            {
                item.stack += amount;
                item.slot.UpdateStack();
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
        Slot slot = Instantiate(itemUI, stackableParent).GetComponent<Slot>();
        Item item = new Item(properties, slot, amt);

        if (slot != null)
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

    private ItemController FindTool(ItemProperties properties)
    {
        for (int i = 0; i < tools.Count; i++)
        {
            if (properties == tools[i].properties)
                return tools[i];
        }

        return null;
    }

    private Item FindItem(ItemProperties item)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].item == item)
                return items[i];
        }

        return null;
    }

    private IEnumerator SwitchTools(ItemController nextTool)
    {
        isEquipping = true;

        if (currentTool != null)
        {
            currentTool.ResetItem();
        }

        float t = nextTool.properties.equipTime;
        while (t > 0.0f) 
        {
            t -= Time.deltaTime * 2f;
            equipSlider.OverrideValue(t / nextTool.properties.equipTime);

            yield return null;
        }

        currentTool = nextTool;
        currentTool.ActivateItem();

        toolIcon.OverrideValue(currentTool.properties.itemSprite);
        equipSlider.OverrideValue(1f);
        toolIcon.SetNatizeSize();

        isEquipping = false;
    }
}

[System.Serializable]
public class Item
{
    public ItemProperties item = null;
    public Slot slot = null;
    public int stack = 0;

    public Item(ItemProperties item, Slot slot, int stack)
    {
        this.item = item;
        this.slot = slot;
        this.stack = stack;
    }
}
