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
        inventory.AddItem(itemProperties, pickupAmount);
    }

    public override void HoverInteract()
    {
    }

    public override void StopHoverInteract()
    {
    }
}
