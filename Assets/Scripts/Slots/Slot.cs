using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

//this class handles the slot functionalilty and slot UI.
public class Slot : InteractableObject, IDragHandler, IEndDragHandler
{
    //refernece to the base slot UI
    [Header("Slot UI")]
    [SerializeField] private Transform uiButtonTransform = null;
    [SerializeField] private Image itemIcon = null;
    [SerializeField] private Text itemInput = null;
    [SerializeField] private Text itemStack = null;

    //reference to the item info UI, when we hover over the item these ui properties will show up
    [Header("Item Info")]
    [SerializeField] private bool controllableItemSlot = false;
    [SerializeField] private Transform itemInfoTransform = null;
    [SerializeField] private Text itemName = null;
    [SerializeField] private Text itemDescription = null;
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color highlighedColor = Color.white;

    //does this slot have an item?
    [HideInInspector]
    public bool hasItem = false;
    //reference to the item
    [HideInInspector]
    public Item item = null;

    //checks if this slot belongs to a player inventory
    private bool isPlayer = false;
    //reference to the slot manager that controls this slot
    private SlotManager slotManager = null;
    //reference to the background image
    private Image slotBackground = null;
    //all the buttons we made that show up when we hover over it
    private List<Button> buttons = new List<Button>();

    private int index = 0;

    [HideInInspector]
    public bool selected = false;

    //create a property of the defaultColor
    public Color dColor
    {
        get
        {
            return defaultColor;
        }

        set
        {
            defaultColor = value;
        }
    }

    //create a property of the highlightedColor
    public Color hColor
    {
        get
        {
            return highlighedColor;
        }

        set
        {
            highlighedColor = value;
        }
    }


    private void Awake()
    {
        //assign variables 
        slotBackground = GetComponent<Image>();

        if (slotBackground)
            defaultColor = slotBackground.color;
    }

    //Method used for setting up slots when theyre first made.
    public void SetupSlot(SlotManager slotManager, bool isPlayer, int index)
    {
        //assign the isPlayer
        this.isPlayer = isPlayer;
        //assign the slotManager
        this.slotManager = slotManager;

        this.index = index;

        //assign the inputText
        itemInput.text = (index + 1).ToString();
        //to make the hierarchy easier to read, rename the slots based on their database type.
        gameObject.name = isPlayer ? "PLAYER_SLOT_" + index : "CHEST_SLOT_" + index;
    }

    //Method used for adding items to a slot, essentially updating its UI to the new item data.
    public void AddItem(Item nextItem)
    {
        //assign the item
        item = nextItem;
        //enable the icon
        itemIcon.enabled = true;
        //assign the sprite
        itemIcon.sprite = item.item.itemSprite;
        //assign the name
        itemName.text = item.item.itemName;
        //assign the description
        itemDescription.text = item.item.itemDescription;
        //assign the slot to this slot
        item.slot = this;

        if (isPlayer)
        {
            //if this is a player, show the item input text because this is the players inventory and we can use buttons to activiate the slot.
            itemInput.enabled = true;
        }

        if (item.item.inventoryButtons && !controllableItemSlot)
        {
            //if this item uses ui buttons, spawn them based on the database type
            if (isPlayer)
            {
                //spawn inventory buttons
                SpawnUIButtons(item.item.uiInventoryButtons);
            }
        }

        if (item.item.chestButtons && !controllableItemSlot)
        {
            if (!isPlayer)
            {
                SpawnUIButtons(item.item.uiChestButtons);
            }
        }


        if (item.item.stackable)
        {
            //if this item is stackable, show the stack text and assign it to the current stack
            itemStack.enabled = true;
            itemStack.text = string.Format("{0}", item.stack);
        }

        //we now have an item
        hasItem = true;
    }

    //method used for spawning in ui buttons to interact with the items
    private void SpawnUIButtons(UIButtonProperties[] properties)
    {
        //loop through all the properties
        for(int i = 0; i < properties.Length; i++)
        {
            //spawn a button
            Button button = Instantiate(item.item.button, uiButtonTransform).GetComponent<Button>();
            //set the text of the button to the property name
            button.GetComponentInChildren<Text>().text = properties[i].name;
            //add this button to the list of buttons
            buttons.Add(button);

            //call the FilterItemButtons method
            FilterItemButtons(button, properties[i]);
            //to start, hide the buttons
            ToggleButtons(false);
        }
    }

    //Method used for filtering out buttons based on the property types.
    private void FilterItemButtons(Button button, UIButtonProperties properties)
    {
        switch (properties.propertyType)
        {
            //if this property type is a use type, when we click the button add the UseItem listener.
            case UIButtonProperties.PropertyTypes.Use:
                button.onClick.AddListener(() => UseItem());
                break;

            //if this property type is a AddToInventory, when we click the button invoke the TakeItem event.
            case UIButtonProperties.PropertyTypes.AddToInventory:
                button.onClick.AddListener(() => GameEvents.OnPlayerTakeItem.Invoke(item));
                break;
        }
    }

    //Method used for resetting the slot back to nothing.
    public void ResetSlot()
    {
        //reset the item slot
        if (item.slot == this)
            item.slot = null;


        //reset all the ui and variables.
        hasItem = false;
        item = null;
        itemIcon.enabled = false;
        slotBackground.color = defaultColor;
        itemIcon.sprite = null;
        itemStack.text = null;
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
        itemInfoTransform.gameObject.SetActive(false);

        //if this is a player inventory, disable the input text.
        if (isPlayer) itemInput.enabled = false;

        //destroy all the buttons
        foreach (Button b in buttons)
        {
            Destroy(b.gameObject);
        }

        //clear the buttons
        buttons.Clear();
    }

    //method used for updating the slot stack ui
    public void UpdateSlotStack(int stack)
    {
        itemStack.text = string.Format("{0}", stack);
    }

    //method used for toggling button ui
    private void ToggleButtons(bool state)
    {
        uiButtonTransform.gameObject.SetActive(state);
    }

    //method used for toggling the item info ui
    private void ToggleItemInfo(bool state)
    {
        if (item == null || item.item == null)
            return;

        itemInfoTransform.gameObject.SetActive(state);
    }

    //method used to let us know if we hover over this object
    public override void HoverInteract()
    {
        //if we hover over this object, show the buttons and item info
        ToggleItemInfo(true);
        ToggleButtons(true);
    }

    //method used for letting us know if we stopped hovering an object
    public override void StopHoverInteract()
    {
        //disable the button and item info
        ToggleItemInfo(false);
        ToggleButtons(false);
    }
    
    //method used for using items
    public void UseItem()
    {
        //call the use item event
        slotManager.TryEnableSlot(index, out item);
    }

    //method used for setting the backgound color
    public void SetColor(Color color, bool selected)
    {
        if (!item.item.controllable)
            return;

        this.selected = selected;
        slotBackground.color = color;
    }

    public override void ButtonInteract()
    {
    }

    //when we are dragging this item, set the icon of it to the mouse position
    public void OnDrag(PointerEventData eventData)
    {
        itemIcon.transform.position = Mouse.current.position.ReadValue();
    }

    //when we stop dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        if (slotManager != null)
        {
            //set the item to the closest slot to the new position.
            slotManager.SetSlotTo(this, itemIcon.transform.position);
        }

        //after set the position to zero.
        itemIcon.transform.localPosition = Vector3.zero;
    }

}
