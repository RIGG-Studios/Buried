using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrapplingHookState : State
{
    private Rigidbody2D physics = null;
    private LineRenderer line = null;
    private GrapplingHookSettings settings = null;
    private Camera camera;

    private Vector2 mouseDir = Vector2.zero;
    private Vector2 grappleTarget = Vector2.zero;
    private HookshotController grappleController = null;
    private bool isGrappling = false;

    [HideInInspector]
    public bool isRetracting = false;

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
            grappleController = (HookshotController)player.itemManagement.FindItemController(player.inventory.FindItem(ItemProperties.WeaponTypes.GrapplingHook));

            line = grappleController.lineRenderer;
        }

        Vector3 mousePos = camera.ScreenToWorldPoint(Utilites.GetMousePosition());
        mouseDir = (mousePos - player.GetPosition()).normalized;

        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, mouseDir, settings.maxDistance, settings.grappleLayer);

        if(hit.collider != null)
        {
            isGrappling = true;
            grappleTarget = hit.point;
            line.enabled = true;
            line.positionCount = 2;

            player.StartCoroutine(GrappleMovement());
        }
        else
        {
            player.stateManager.TransitionStates(PlayerStates.Movement);
        }
    }

    public override void UpdateLogic()
    {
        if (isRetracting)
        {
            Vector2 grapplePos = Vector2.Lerp(player.transform.position, grappleTarget, settings.speed * Time.deltaTime);
            player.transform.position = grapplePos;

            line.SetPosition(0, player.GetPosition());

            if(Vector3.Distance(player.GetPosition(), grappleTarget) < 0.5f)
            {
                isRetracting = false;
                isGrappling = false;
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
        isRetracting = true;
    }
}
