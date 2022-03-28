using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<ItemProperties> startingTools = new List<ItemProperties>();
    [SerializeField] private GameObject itemUI = null;
    [SerializeField] private Transform itemParent = null;

    public bool initialized { get; private set; }
    public List<ItemController> tools { get; private set; }
    public List<Item> items { get; private set; }
    public ItemController currentTool { get; private set; }

    private Player player = null;
    private UIElementGroup equipGroup = null;
    private SliderElement equipSlider = null;
    private UIElement equipText = null;
    private Transform stackableParent = null;

    private int currentToolIndex = 0;
    private float equipTimer = 0.0f;
    private float equipLength = 0.0f;
    private bool isEquipping = false;

    private void Awake()
    {
        tools = new List<ItemController>();
        items = new List<Item>();

        player = GetComponent<Player>();

        equipGroup = CanvasManager.instance.FindElementGroupByID("EquipGroup");

        if (equipGroup != null)
        {
            equipSlider = (SliderElement)equipGroup.FindElement("slider");
            equipText = equipGroup.FindElement("text");
        }

        stackableParent = CanvasManager.instance.FindElementGroupByID("PlayerInventory").FindElement("grid").transform;
        InitializeInventory();
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
                tools.Add(controller);
            }
        }

        player.playerInput.Player.Fire.performed += ctx => UseTool();

        SwitchItems(0);
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
        if (isEquipping)
            return;

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
            bool success = AddStackableItem(properties, amount);

            if (success)
            {
                if (properties.itemType == ItemProperties.ItemTypes.Note)
                    GameEvents.OnNotePickedUp?.Invoke(FindItem(properties).stack);

                return success;
            }
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
        equipGroup.UpdateElements(0, 0, true);
        equipSlider.SetMax(nextTool.properties.equipTime);

        if (currentTool != null)
        {
            currentTool.ResetItem();
        }

        float t = nextTool.properties.equipTime;
        while (t > 0.3f) 
        {
            t -= Time.deltaTime * 2f;
            equipText.OverrideValue("Equipping: " + nextTool.properties.itemName);
            equipSlider.OverrideValue(t);

            yield return null;
        }

        equipGroup.UpdateElements(0, 0, false);

        currentTool = nextTool;
        currentTool.ActivateItem();

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
