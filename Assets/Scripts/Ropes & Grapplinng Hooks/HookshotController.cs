using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotController : ItemController
{
    [SerializeField] private LineRenderer lineRenderer = null;

    private int bulletStack;

    public override void UseItem()
    {
        Item ammo = null;
        player.inventory.TryFindItem(ItemProperties.ItemTypes.GrapplingHookAmmo, out ammo);
        if (ammo != null && ammo.stack > 0)
        {
            if (player.stateManager.currentState.name == "PlayerGrapple")
                return;

            Vector2 mouseDir = (Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition()) - player.GetPosition()).normalized;
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, mouseDir, player.grappleHookSettings.maxDistance, player.grappleHookSettings.grappleLayer);

            if (hit.collider != null)
            {
                player.stateManager.TransitionStates(PlayerStates.Grappling);
            }

            player.inventory.UseItem(ammo.item);
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
