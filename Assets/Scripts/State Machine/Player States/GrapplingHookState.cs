using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrappleStates
{
    None,
    Shooting,
    Retracting
}

public class GrapplingHookState : State
{
    public GrappleStates state = GrappleStates.None;

    private Rigidbody2D physics = null;
    private LineRenderer line = null;
    private GrapplingHookSettings settings = null;
    private Camera camera = null;
    private HookshotController grappleController = null;


    private Vector2 mouseDir = Vector2.zero;
    private Vector2 grappleTarget = Vector2.zero;

    public GrapplingHookState(Player player) : base("PlayerGrapple", player)
    {
        this.player = player;

        camera = Camera.main;
        physics = player.GetComponent<Rigidbody2D>();
        settings = player.grappleHookSettings;
    }

    public override void EnterState()
    {
        if (grappleController == null)
        {
            grappleController = (HookshotController)player.itemManagement.GetActiveController();

            line = grappleController.GetLine();
        }

        Vector3 mousePos = camera.ScreenToWorldPoint(Utilites.GetMousePosition());
        mouseDir = (mousePos - player.GetPosition()).normalized;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, mouseDir, settings.maxDistance, settings.grappleLayer);

        if(hit.collider != null)
        {
            state = GrappleStates.Shooting;
            grappleTarget = hit.point;
            line.enabled = true;
            line.positionCount = 2;

            player.StartCoroutine(GrappleMovement());
        }
        else
        {
            player.stateManager.TransitionStates(PlayerStates.Movement);
        }

        player.collider.enabled = false;
    }
    public override void ExitState()
    {
        player.collider.enabled = true;
    }

    public override void UpdateLogic()
    {
        if (state == GrappleStates.Retracting)
        {
            Vector2 grapplePos = Vector2.Lerp(player.transform.position, grappleTarget, settings.speed * Time.deltaTime);
            player.transform.position = grapplePos;

            line.SetPosition(0, grappleController.GetPosition());

            float dist = Vector3.Distance(player.GetPosition(), grappleTarget);

            if (dist< 0.5f)
            {
                state = GrappleStates.None;
                line.enabled = false;

                player.stateManager.TransitionStates(PlayerStates.Movement);
            }
        }
    }

    public IEnumerator GrappleMovement()
    {
        float t = 0;
        float time = 10;

        line.SetPosition(0, grappleController.GetPosition());
        line.SetPosition(1, grappleController.GetPosition());

        Vector2 nextPos;

        for(; t < time; t += settings.shootSpeed * Time.deltaTime)
        {
            nextPos = Vector2.Lerp(grappleController.GetPosition(), grappleTarget, t / time);
            line.SetPosition(0, grappleController.GetPosition());
            line.SetPosition(1, nextPos);
            yield return null;
        }

        line.SetPosition(1, grappleTarget);
        state = GrappleStates.Retracting;
    }
}
