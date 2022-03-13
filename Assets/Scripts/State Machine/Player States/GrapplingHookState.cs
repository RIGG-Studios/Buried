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
    private Vector3 grappleTarget = Vector2.zero;

    private float moveTime;
    private float waveSize;
    private bool straighenLine;

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
            grappleController = (HookshotController)player.itemManagement.activeController;

            line = grappleController.GetLine();
        }

        line.positionCount = settings.percision;

        Vector3 mousePos = camera.ScreenToWorldPoint(Utilites.GetMousePosition());
        mouseDir = (mousePos - player.GetPosition()).normalized;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, mouseDir, settings.maxDistance, settings.grappleLayer);

        if(hit.collider != null)
        {
            float dist = (new Vector3(hit.point.x, hit.point.y, 0) - player.GetPosition()).magnitude;

            if (dist > settings.maxDistance)
            {
                player.stateManager.TransitionStates(PlayerStates.Movement);
                return;
            }

            state = GrappleStates.Shooting;
            grappleTarget = hit.point;
            line.enabled = true;
        }
        else
        {
            player.stateManager.TransitionStates(PlayerStates.Movement);
        }

        waveSize = settings.waveSize;
        player.collider.enabled = false;
    }
    public override void ExitState()
    {
        player.collider.enabled = true;
        moveTime = 0.0f;
    }

    public override void UpdateLogic()
    {
        moveTime += Time.deltaTime;

        if(state == GrappleStates.Shooting)
        {
            if (line.GetPosition(settings.percision - 1).x != grappleTarget.x)
                MoveLine();
            else
            {
                state = GrappleStates.Retracting;
            }
        }

        if (state == GrappleStates.Retracting)
        {
            if(waveSize > 0)
            {
                waveSize -= Time.deltaTime * settings.straightenLineSpeed;
                MoveLine();
            }
            else
            {
                MoveToTarget();
                waveSize = 0;

                float dist = Vector3.Distance(player.GetPosition(), grappleTarget);

                if (dist < 0.5f)
                {
                    state = GrappleStates.None;
                    line.enabled = false;

                    player.stateManager.TransitionStates(PlayerStates.Movement);
                }
            }
        }
    }

    public void MoveLine()
    {
        for (int i = 0; i < settings.percision; i++)
        {
            float delta = (float)i / ((float)settings.percision - 1f);
            Vector2 offset = Vector2.Perpendicular(grappleController.GetPosition() - grappleTarget).normalized * settings.ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(grappleController.GetPosition(), grappleTarget, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(grappleController.GetPosition(), targetPosition, settings.ropeLaunchSpeedCurve.Evaluate(moveTime) * settings.launchSpeedMultiplier);

            line.SetPosition(i, currentPosition);
        }
    }

    private void MoveToTarget()
    {
        line.positionCount = 2;
        line.SetPosition(0, grappleTarget);
        line.SetPosition(1, grappleController.GetPosition());

        player.transform.position = Vector3.Lerp(player.transform.position, grappleTarget, Time.deltaTime * settings.shootSpeed);
    }
}
