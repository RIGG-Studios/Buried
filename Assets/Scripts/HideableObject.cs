using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : InteractableObject
{

    public Transform hidePosition = null;

    private Player player
    {
        get
        {
            return FindObjectOfType<Player>();
        }
    }

    public override void ButtonInteract()
    {
        GameEvents.OnToggleHidePlayer?.Invoke();
    }

    public override void HoverInteract()
    {
        if (player.stateManager.GetStateInEnum() == PlayerStates.Hiding)
        {
            useAssist = false;
        }
        else
        {
            useAssist = true;
        }
    }

    public override void StopHoverInteract()
    {
    }
}
