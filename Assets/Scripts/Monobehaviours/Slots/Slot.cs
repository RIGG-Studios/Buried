using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : InteractableObject
{
    public bool occupied { get; private set; }
    public Item item { get; private set; }

    public Image itemIcon;
    public Text itemStack;
    public Transform uiButtonTransform;

    Inventory inventory;
    int stack;
    List<Button> buttons = new List<Button>();

    private void Start()
    {
       itemStack.enabled = false;
        itemIcon.enabled = false;
    }

    public void SetupSlotWithItem(Inventory inventory, Item item, bool inventoryButtons)
    {
        itemIcon.enabled = true;
        this.item = item;
        this.inventory = inventory;

        if (item.useInventoryButtons && inventoryButtons)
        {
            for(int i = 0; i < item.uiButtons.Length; i++)
            {
                Button button = Instantiate(item.uiButton, uiButtonTransform).GetComponent<Button>();

                button.GetComponentInChildren<Text>().text = item.uiButtons[i].name;
                buttons.Add(button);

                switch (item.uiButtons[i].propertyType)
                {
                    case UIButtonProperties.PropertyType.ShowProperty:

                        string property = item.uiButtons[i].propertyName;
                        button.onClick.AddListener(() => inventory.ShowItemProperty(item, property));
                        break;

                    case UIButtonProperties.PropertyType.Use:
                        button.onClick.AddListener(() => inventory.UseItem(item));
                        break;

                    case UIButtonProperties.PropertyType.Discard:
                        button.onClick.AddListener(() => inventory.DiscardItem(item));
                        break;
                }

                ToggleButtons(false);
            }
        }

        itemIcon.sprite = item.itemSprite;

        if (item.stackable)
        {
            itemStack.enabled = true;
            this.stack = 1;
            itemStack.text = string.Format("x{0}", 1);
        }

        occupied = true;
    }

    public void ResetSlot()
    {
        occupied = false;
        item = null;
        inventory = null;
        itemIcon.enabled = false;
        itemIcon.sprite = null;
        itemStack.text = null;

        foreach (Button b in buttons)
            Destroy(b.gameObject);

        buttons.Clear();
    }

    public void UpdateSlotStack(int stack)
    {
        this.stack += stack;
        itemStack.text = string.Format("x{0}", this.stack);
    }

    public override void Interact(Player player)
    {
        ToggleButtons(true);
    }

    public override void StopInteract()
    {
        ToggleButtons(false);
    }

    private void ToggleButtons(bool state) => uiButtonTransform.gameObject.SetActive(state);

    public int GetStack() => stack;
}
