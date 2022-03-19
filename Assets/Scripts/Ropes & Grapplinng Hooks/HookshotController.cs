using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotController : ItemController
{
    [SerializeField] private LineRenderer lineRenderer = null;

    private int bulletStack;
    private GrapplingHookSettings settings;
    public override void SetupController(Player player, Item itemInInventory)
    {
        base.SetupController(player, itemInInventory);
        settings = player.grappleHookSettings;
    }

    public override void UseItem()
    {
        Item ammo = null;
        player.inventory.TryFindItem(ItemProperties.ItemTypes.GrapplingHookAmmo, out ammo);

        if (ammo != null && ammo.stack > 0)
        {
            if (player.stateManager.currentState.name == "PlayerGrapple")
                return;

            Vector2 mouseDir = (Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition()) - player.GetPosition()).normalized;
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, mouseDir, settings.maxDistance, settings.grappleLayer);

            if (hit.collider != null)
            {
                player.stateManager.TransitionStates(PlayerStates.Grappling);
            }

            player.inventory.UseItem(ammo.item);
        }
        else
        {
            player.playerCam.ShakeCamera(settings.shakeDuration / 3, settings.shakeMagnitude / 3);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public LineRenderer GetLine()
    {
        return lineRenderer;
    }
}
