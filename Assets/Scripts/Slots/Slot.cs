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
    public Item item;

    private bool isPlayer;


    public void SetupSlot(ItemDatabase database, bool isPlayer)
    {
        this.database = database;
        this.isPlayer = isPlayer;
    }

    public void AddItem(Item item, int amount)
    {
        itemIcon.enabled = true;
        itemIcon.sprite = item.item.itemSprite;

        this.item = item;

        if (item.item.useUIButtons)
        {
            if (isPlayer) SpawnUIButtons(item.item.uiInventoryButtons);
            else SpawnUIButtons(item.item.uiChestButtons);
        }

        if (item.item.stackable)
        {
            itemStack.enabled = true;
            itemStack.text = string.Format("x{0}", amount);
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
        //        button.onClick.AddListener(() => inventory.UseItem(item));
                break;

            case UIButtonProperties.PropertyTypes.Discard:
          //      button.onClick.AddListener(() => inventory.DiscardItem(item));
                break;

            case UIButtonProperties.PropertyTypes.AddToInventory:
                button.onClick.AddListener(() => GameEvents.OnPlayerTakeItem.Invoke(item));
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


    private void ToggleButtons(bool state)
    {
        uiButtonTransform.gameObject.SetActive(state);
    }

    public override void HoverInteract()
    {
        ToggleButtons(true);
    }

    public override void StopHoverInteract()
    {
        ToggleButtons(false);
    }

    public override void ButtonInteract()
    {
    }
}
