using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : InteractableObject
{
    public bool hidingPlayer;
    private GameObject player
    {
        get
        {
            return FindObjectOfType<Player>().gameObject;
        }
    }

    public override void ButtonInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void HoverInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void StopHoverInteract()
    {
        throw new System.NotImplementedException();
    }
}
