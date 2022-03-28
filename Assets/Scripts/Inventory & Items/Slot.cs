using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public bool initialized { get; private set; }

    public Image icon = null;
    public Text stack = null;

    private Item item = null;

    public void SetupSlot(Item item)
    {
        icon.sprite = item.item.itemSprite;
        icon.enabled = true;

        if (item.item.stackable)
        {
            stack.text = item.stack.ToString();
            stack.enabled = true;
        }


        this.item = item;
        initialized = true;
    }

    public void UpdateStack()
    {
        if (item == null)
            return;

        stack.text = item.stack.ToString();
    }
}
