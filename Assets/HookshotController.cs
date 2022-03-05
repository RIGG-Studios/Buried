using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotController : ItemController
{
    public GrapplingHookSettings settings;
    public LineRenderer lineRenderer = null;

    public override void UseItem()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition());
        Vector2 mouseDir = (mousePos - player.GetPosition()).normalized;

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, mouseDir, settings.maxDistance, settings.grappleLayer);

        if (hit.collider != null)
        {
            player.stateManager.TransitionStates(PlayerStates.Grappling);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
