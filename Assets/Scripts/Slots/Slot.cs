using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Slot : InteractableObject, IDragHandler, IEndDragHandler
{
    [Header("Buttons")]
    [SerializeField] private Transform uiButtonTransform = null;
    [SerializeField] private Image itemIcon = null;
    [SerializeField] private Text itemInput = null;
    [SerializeField] private Text itemStack = null;

    [Header("Item Info")]
    [SerializeField] private Transform itemInfoTransform = null;
    [SerializeField] private Text itemName = null;
    [SerializeField] private Text itemDescription = null;
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color highlighedColor = Color.white;

    [HideInInspector]
    public bool selected = false;
    [HideInInspector]
    public bool hasItem = false;
    [HideInInspector]
    public Item item = null;

    private bool isPlayer = false;
    private SlotManager slotManager = null;
    private Image slotBackground = null;
    private List<Button> buttons = new List<Button>();

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
        slotBackground = GetComponent<Image>();

        if (slotBackground)
            defaultColor = slotBackground.color;
    }

    public void SetupSlot(SlotManager slotManager, bool isPlayer, int index)
    {
        this.isPlayer = isPlayer;
        this.slotManager = slotManager;

        itemInput.text = (index + 1).ToString();
        gameObject.name = isPlayer ? "PLAYER_SLOT_" + index : "CHEST_SLOT_" + index;
    }

    public void AddItem(Item nextItem)
    {
        item = nextItem;
        itemIcon.enabled = true;
        itemIcon.sprite = item.item.itemSprite;
        itemName.text = item.item.itemName;
        itemDescription.text = item.item.itemDescription;
        item.slot = this;
        Debug.Log(item.slot);

        if (isPlayer)
        {
            itemInput.enabled = true;
        }

        if (item.item.useUIButtons)
        {
            if (isPlayer)
            {
                SpawnUIButtons(item.item.uiInventoryButtons);
            }
            else
            {
                SpawnUIButtons(item.item.uiChestButtons);
            }
        }

        if (item.item.stackable)
        {
            itemStack.enabled = true;
            itemStack.text = string.Format("{0}", item.stack);
        }

        hasItem = true;
    }

    private void SpawnUIButtons(UIButtonProperties[] properties)
    {
        for(int i = 0; i < properties.Length; i++)
        {
            Button button = Instantiate(item.item.uiButton, uiButtonTransform).GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = properties[i].name;
            buttons.Add(button);

            FilterItemButtons(button, properties[i]);
            ToggleButtons(false);
        }
    }

    private void FilterItemButtons(Button button, UIButtonProperties properties)
    {
        switch (properties.propertyType)
        {
            case UIButtonProperties.PropertyTypes.Use:
                button.onClick.AddListener(() => UseItem());
                break;

            case UIButtonProperties.PropertyTypes.AddToInventory:
                button.onClick.AddListener(() => GameEvents.OnPlayerTakeItem.Invoke(item));
                break;
        }
    }

    public void ResetSlot()
    {
        hasItem = false;
        if(item.slot == this)
            item.slot = null;

        item = null;
        itemIcon.enabled = false;
        slotBackground.color = defaultColor;
        itemIcon.sprite = null;
        itemStack.text = null;
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
        itemInfoTransform.gameObject.SetActive(false);

        if (isPlayer) itemInput.enabled = false;

        foreach (Button b in buttons)
        {
            Destroy(b.gameObject);
        }

        buttons.Clear();
    }

    public void UpdateSlotStack(int stack)
    {
        itemStack.text = string.Format("{0}", stack);
    }

    private void ToggleButtons(bool state)
    {
        uiButtonTransform.gameObject.SetActive(state);
    }

    private void ToggleItemInfo(bool state)
    {
        if (item == null || item.item == null)
            return;

        itemInfoTransform.gameObject.SetActive(state);
    }

    public override void HoverInteract()
    {
        ToggleItemInfo(true);
        ToggleButtons(true);
    }

    public override void StopHoverInteract()
    {
        ToggleItemInfo(false);
        ToggleButtons(false);
    }
    public void UseItem()
    {
        GameEvents.OnPlayerUseItem(item.item);
    }

    public void SetColor(Color color)
    {
        slotBackground.color = color;
    }

    public override void ButtonInteract()
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemIcon.transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slotManager != null)
        {
            slotManager.SetSlotTo(this, itemIcon.transform.position);
        }

        itemIcon.transform.localPosition = Vector3.zero;
    }

}
