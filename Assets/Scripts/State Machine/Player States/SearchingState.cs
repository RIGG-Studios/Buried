using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingState : State
{
    private Chest currentChest = null;
    private Vector2 movement;

    public SearchingState(Player player) : base("PlayerSearching", player) => this.player = player;

    public override void EnterState()
    {
        player.playerInput.Player.Leave.performed += ctx => ExitChest();
        GameEvents.OnPlayerTakeItem += TakeItem;

        currentChest = (Chest)player.playerInteraction.hoveredObject;
        player.playerInteraction.allowInteractions = false;
    }

    public override void ExitState()
    {
    }

    public override void UpdateInput()
    {
        movement = player.playerInput.Player.Move.ReadValue<Vector2>();
    }

    public override void UpdateLogic()
    {
        if (movement != Vector2.zero)
            ExitChest();
    }

    private void TakeItem(Item item)
    {
        player.inventory.AddNewItem(item, currentChest.inventory);
    }

    private void ExitChest()
    {
        currentChest.StopButtonInteract();
        player.playerInteraction.allowInteractions = true;
        player.stateManager.TransitionStates(PlayerStates.Movement); 
        GameEvents.OnPlayerTakeItem -= TakeItem;

    }
}
