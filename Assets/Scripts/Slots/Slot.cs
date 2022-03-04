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

    private List<Button> buttons = new List<Button>();


    [HideInInspector]
    public bool hasItem;
    [HideInInspector]
    public Item item;

    private bool isPlayer = false;
    private SlotManager slotManager = null;

    public void SetupSlot(SlotManager slotManager, bool isPlayer, int index)
    {
        this.isPlayer = isPlayer;
        this.slotManager = slotManager;
        itemInput.text = (index + 1).ToString();
        gameObject.name = isPlayer ? "PLAYER_SLOT_" + index : "CHEST_SLOT_" + index;
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemIcon.transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slotManager != null)
        {
            Slot[] slots = slotManager.GetAllSlots();

            float closestDist = -1f;
            Slot closestSlot = null;

            for (int i = 0; i < slots.Length; i++)
            {
                float dist = Vector2.Distance(itemIcon.transform.position, slots[i].transform.position);
                if (closestSlot == null || dist < closestDist)
                {
                    closestSlot = slots[i];
                    closestDist = dist;
                }
            }

            if (closestSlot != null && closestDist <= 25f)
            {
                slotManager.SwitchItemsInSlots(this, closestSlot);
            }
        }

        itemIcon.transform.localPosition = Vector3.zero;
    }

    public void AddItem(Item item)
    {
        itemIcon.enabled = true;
        itemIcon.sprite = item.item.itemSprite;
        itemName.text = item.item.itemName;
        itemDescription.text = item.item.itemDescription;
        this.item = item;

        if (isPlayer)
        {
            itemInput.enabled = true;
        }

        if (item.item.useUIButtons)
        {
            if (isPlayer) SpawnUIButtons(item.item.uiInventoryButtons);
            else SpawnUIButtons(item.item.uiChestButtons);
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
            case UIButtonProperties.PropertyTypes.ShowProperty:
        //        button.onClick.AddListener(() => inventory.ShowItemProperty(item, properties.propertyName));
                break;

            case UIButtonProperties.PropertyTypes.Use:
                button.onClick.AddListener(() => UseItem());
                break;

            case UIButtonProperties.PropertyTypes.Discard:
          //      button.onClick.AddListener(() => inventory.DiscardItem(item));
                break;

            case UIButtonProperties.PropertyTypes.AddToInventory:
                button.onClick.AddListener(() => GameEvents.OnPlayerTakeItem.Invoke(item));
                break;
        }
    }

    public void UseItem()
    {
        if (!hasItem)
            return;

        GameEvents.OnPlayerUseItem(item.item);
    }

    public void ResetSlot()
    {
        hasItem = false;
        item = null;
        itemIcon.enabled = false;
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

    public override void ButtonInteract()
    {
    }
}
