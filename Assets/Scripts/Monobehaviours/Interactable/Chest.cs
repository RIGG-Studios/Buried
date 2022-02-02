using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    public GameObject closedChest;
    public GameObject openChest;
    public override void Interact()
    {
        openChest.SetActive(true);
        closedChest.SetActive(false);
        canInteract = false;
    }
}
