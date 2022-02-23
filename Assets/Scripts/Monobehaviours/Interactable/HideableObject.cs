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

    public override void Interact(Player player)
    {
        if (hidingPlayer)
        {
            player.DoAction(PlayerActions.Hide);
            interactionType = InteractionType.Hide;
            hidingPlayer = false;
        }
        else
        {
            player.DoAction(PlayerActions.Hide);
            player.transform.position = transform.position;
            interactionType = InteractionType.UnHide;
            hidingPlayer = true;
        }
    }

    public override void StopInteract()
    {
    }
}