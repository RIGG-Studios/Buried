using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public GameObject closedDoor;
    public GameObject openDoor;
    public Collider2D collider;
    bool doorClosed = true;
    public override void Interact()
    {
        if (doorClosed)
        {
            collider.enabled = false;
            closedDoor.SetActive(false);
            openDoor.SetActive(true);
            doorClosed = !doorClosed;
        }
        else
        {
            collider.enabled = false;
            closedDoor.SetActive(true);
            openDoor.SetActive(false);
            doorClosed = !doorClosed;
        }
    }

    public override void StopInteract()
    {
        throw new System.NotImplementedException();
    }
}
