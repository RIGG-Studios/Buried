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
    private PlayerFootsteps footSteps; 
    private Animator animator;

    private float stepCooldown;

    public MovementState(Player player) : base("PlayerMovement", player) => this.player = player;

    public override void EnterState()
    {
        camera = Camera.main;
        settings = player.movementSettings;
        physics = player.GetComponent<Rigidbody2D>();
        footSteps = player.GetComponent<PlayerFootsteps>();
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

        bool isMoving = movementInput != Vector2.zero;

        if (isMoving && stepCooldown < 0f)
        {
            footSteps.PlayFootstep();
            stepCooldown = settings.stepRate / GetMovementSpeed() / 2f;
        }

        cameraController.SetOffset(dir * settings.cameraOffset);
        animator.SetInteger("Direction", Utilites.DirectionToIndex(dir, 4));

        movementInput = moveAction.ReadValue<Vector2>();
        stepCooldown -= Time.deltaTime;
    }

    public override void UpdatePhysics()
    {
        physics.MovePosition(physics.position + movementInput * GetMovementSpeed() * Time.fixedDeltaTime);
    }

    private float GetMovementSpeed()
    {
        float speed = settings.movementSpeed + player.paranoidManager.paranoidAmount * settings.movementParanoidMultiplier;

        return speed;
    }
}
