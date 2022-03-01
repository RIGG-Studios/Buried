using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : InteractableObject
{
    [SerializeField] private Transform uiButtonTransform = null;
    [SerializeField] private Image itemIcon = null;
    [SerializeField] private Text itemStack = null;

    private ItemDatabase database = null;
    private List<Button> buttons = new List<Button>();

    [HideInInspector]
    public bool hasItem;
    [HideInInspector]
    public ItemProperties item;

    private void Awake()
    {
        itemIcon.enabled = false;
        itemStack.enabled = false;
    }

    public void SetupSlot(ItemDatabase database)
    {
        this.database = database;
    }

    public void AddItem(ItemProperties item, bool inventoryButtons, int amount)
    {
        Debug.Log("wa");
        itemIcon.sprite = item.itemSprite;
        itemIcon.enabled = true;

        this.item = item;

        if (item.useInventoryButtons && inventoryButtons)
        {
            for(int i = 0; i < item.uiButtons.Length; i++)
            {
                Button button = Instantiate(item.uiButton, uiButtonTransform).GetComponent<Button>();
                button.GetComponentInChildren<Text>().text = item.uiButtons[i].name;
                buttons.Add(button);

                FilterItemButtons(button, item.uiButtons[i]);
                ToggleButtons(false);
            }
        }

        if (item.stackable)
        {
            itemStack.enabled = true;
            itemStack.text = string.Format("x{0}", amount);
        }

        hasItem = true;
    }

    private void FilterItemButtons(Button button, UIButtonProperties properties)
    {
        switch (properties.propertyType)
        {
            case UIButtonProperties.PropertyType.ShowProperty:
        //        button.onClick.AddListener(() => inventory.ShowItemProperty(item, properties.propertyName));
                break;

            case UIButtonProperties.PropertyType.Use:
        //        button.onClick.AddListener(() => inventory.UseItem(item));
                break;

            case UIButtonProperties.PropertyType.Discard:
          //      button.onClick.AddListener(() => inventory.DiscardItem(item));
                break;
        }
    }

    public void ResetSlot()
    {
        hasItem = false;
        item = null;
        database = null;
        itemIcon.enabled = false;
        itemIcon.sprite = null;
        itemStack.text = null;

        foreach (Button b in buttons)
        {
            Destroy(b.gameObject);
        }

        buttons.Clear();
    }

    public void UpdateSlotStack(int stack)
    {
        itemStack.text = string.Format("x{0}", stack);
    }

    public override void Interact(Player player)
    {
        ToggleButtons(true);
    }

    public override void StopInteract()
    {
        ToggleButtons(false);
    }

    private void ToggleButtons(bool state)
    {
        uiButtonTransform.gameObject.SetActive(state);
    }
}
