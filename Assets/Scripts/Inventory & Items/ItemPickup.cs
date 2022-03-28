using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : InteractableObject
{  
    [SerializeField] private ItemProperties itemProperties = null;
    [SerializeField, Range(1, 5)] private int pickupAmount;


    private PlayerInventory inventory
    {
        get
        {
            return FindObjectOfType<PlayerInventory>();
        }
    }
    public override void ButtonInteract()
    {
        if (itemProperties.itemType == ItemProperties.ItemTypes.Note)
        {
            NoteReadingManager.instance.ReadNote(itemProperties);
            Destroy(gameObject);
            return;
        }

        bool itm = inventory.AddItem(itemProperties, pickupAmount);

        if(itm)
        {
            Destroy(gameObject);
        }
    }

    public override void HoverInteract()
    {
    }

    public override void StopHoverInteract()
    {
    }
}
