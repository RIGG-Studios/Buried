using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : InteractableObject
{
    public ItemProperties item;

    private Inventory inventory
    {
        get
        {
            return FindObjectOfType<Inventory>();
        }
    }

    public override void Interact(Player player)
    {
      //  inventory.AddItem(item);

        Destroy(gameObject);
    }

    public override void StopInteract()
    {
    }
}
