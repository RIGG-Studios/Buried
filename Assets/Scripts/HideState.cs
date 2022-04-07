using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : State
{
    private Camera camera = null;
    private Vector3 mouseDir = Vector3.zero;

    private UIElementGroup leaveUI = null;

    public HideState(Player player) : base("PlayerHide", player)
    {
        this.player = player;
        camera = Camera.main;
        leaveUI = player.playerCanvas.FindElementGroupByID("LeaveGroup");
    }

    public override void EnterState()
    {
        player.render.enabled = false;
        player.collider.enabled = false;
        leaveUI.UpdateElements(0, 0, true);
    }

    public override void UpdateInput()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(Utilites.GetMousePosition());
        mouseDir = (mousePos - player.GetPosition()).normalized;

        player.playerInput.Player.Leave.performed += ctx => LeaveHideableObject();
    }

    public override void UpdateLogic()
    {
        player.playerCam.SetOffset(mouseDir * 6f);
    }


    public override void ExitState()
    {
        player.render.enabled = true;
        player.collider.enabled = true;
        leaveUI.UpdateElements(0, 0, false);
    }

    private void LeaveHideableObject()
    {
        if (player.stateManager.GetStateInEnum(player.stateManager.currentState) != PlayerStates.Hiding)
            return;

        GameEvents.OnToggleHidePlayer?.Invoke();
    }
}
