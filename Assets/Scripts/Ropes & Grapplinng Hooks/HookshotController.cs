using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotController : ItemController
{
    [SerializeField] private LineRenderer lineRenderer = null;

    private GrapplingHookSettings settings;

    public override void SetupController(Player player, ItemProperties itemInInventory)
    {
        base.SetupController(player, itemInInventory);
        settings = player.grappleHookSettings;
    }

    public override void UseItem()
    {
        if (!CanUseItem())
            return;

        Item ammo = null;
        player.inventory.HasItem(ItemProperties.ItemTypes.GrapplingHookAmmo, out ammo);

        if (ammo != null && player.stateManager.currentState.name != "PlayerGrapple")
        {
            if(ammo.stack <= 0)
            {
                StartCoroutine(ShowAmmoNeededUI("NO AMMO"));
                return;
            }

            Vector2 playerPos = player.GetPosition();
            Vector2 camPos = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition());
            Vector2 mouseDir = (camPos - playerPos).normalized;
            RaycastHit2D hit = Physics2D.Raycast(camPos, mouseDir, 0.1f, settings.grappleLayer);

            if (hit.collider != null)
            {
                float dist = (playerPos - hit.point).magnitude;

                if (dist > settings.maxDistance || CheckForWall(mouseDir, dist))
                    return;

                player.stateManager.TransitionStates(PlayerStates.Grappling);
            }
        }
        else
        {
            StartCoroutine(ShowAmmoNeededUI("NO AMMO"));
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

    private bool CheckForWall(Vector2 mouseDir, float dist)
    {
        RaycastHit2D hit = Physics2D.Raycast(player.GetPosition(), mouseDir, dist, settings.wallLayer);

        return hit.collider != null;
    }
}
