using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : InteractableObject
{
    public GameObject inventoryButton;

    public override void Interact()
    {
        inventoryButton.SetActive(true);
    }
}
