using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : InteractableObject
{
    public bool occupied { get; private set; }

    public Image itemIcon;
    public Text itemStack;
    public Transform uiButtonTransform;

    Item item;

    public void SetupSlotWithItem(Item item)
    {
        this.item = item;

        if (item.useInventoryButtons)
        {
            for(int i = 0; i < item.uiButtons.Length; i++)
            {
                GameObject button = Instantiate(item.uiButton, uiButtonTransform);
                button.GetComponentInChildren<Text>().text = item.uiButtons[i].name;

                ToggleButtons(false);
            }
        }

        itemIcon.sprite = item.itemSprite;

        if(item.stackable)
            itemStack.text = string.Format("x{0}", 1);

        occupied = true;
    }

    public void UpdateSlotStack(int stack)
    {
        if (itemStack == null)
            return;

        if (item.stackable)
            itemStack.text = string.Format("x{0}", stack);
    }

    public void ToggleButtons(bool state) => uiButtonTransform.gameObject.SetActive(state);

    public override void Interact()
    {
        ToggleButtons(true);
    }

    public override void StopInteract()
    {
        ToggleButtons(false);
    }
}
