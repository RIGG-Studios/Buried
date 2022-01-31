using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputActions inputActions;

    PlayerMovement movement;
    GrapplingHook grappleHook;

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Fire.started += ctx => GrappleCheck();
        inputActions.Player.Fire.canceled += ctx => EndGrapple();
        inputActions.Player.Enable();
    }

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        grappleHook = GetComponent<GrapplingHook>();
    }

    private void Update()
    {
        Vector2 movementInput = inputActions.Player.Move.ReadValue<Vector2>();

        movement.UpdateInputVector(movementInput);
    }

    private void GrappleCheck()
    {
        if (grappleHook.TryStartGrapple())
            movement.enabled = false;
    }

    private void EndGrapple()
    {
        if (grappleHook.TryEndGrapple())
            movement.enabled = true;
    }
}

