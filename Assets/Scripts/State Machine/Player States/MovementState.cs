using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : State
{
    private Vector2 movementInput;

    private Camera camera;
    private PlayerCamera cameraController;
    private MovementSettings settings;
    private Rigidbody2D physics;
    private Animator animator;

    public MovementState(Player player) : base("PlayerMovement", player) => this.player = player;

    public override void EnterState()
    {
        camera = Camera.main;
        settings = player.movementSettings;
        physics = player.GetComponent<Rigidbody2D>();
        animator = player.animator;

        if(camera != null)
        {
            cameraController = camera.GetComponent<PlayerCamera>();
        }

        cameraController.SetTarget(player.transform);
    }

    public override void UpdateLogic()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(Utilites.GetMousePosition());
        Vector2 dir = (mousePos - player.GetPosition()).normalized;

        cameraController.SetOffset(dir * settings.cameraOffset);
        movementInput = moveAction.ReadValue<Vector2>();

        animator.SetInteger("Direction", Utilites.DirectionToIndex(dir, 4));
    }

    public override void UpdatePhysics()
    {
        physics.MovePosition(physics.position + movementInput * settings.movementSpeed * Time.fixedDeltaTime);
    }
}
