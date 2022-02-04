using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Player player
    {
        get
        {
            return GetComponent<Player>();
        }
    }

    private PlayerMovement playerMovement
    {
        get
        {
            return GetComponent<PlayerMovement>();
        }
    }

    InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();

        inputActions.Player.Fire.started += ctx => OnFireAction();
        inputActions.Player.Fire.canceled += ctx => OnFireActionReleased();
        inputActions.Player.Flashlight.performed += ctx => OnFlashlightAction();
        inputActions.Player.Interact.performed += ctx => OnInteracAction();

        inputActions.Player.Enable();
    }

    private void Update()
    {
        Vector2 movementInput = inputActions.Player.Move.ReadValue<Vector2>();

        playerMovement.UpdateInputVector(movementInput);
    }

    private void OnFireAction() => player.TryUseGrapplingHook();
    private void OnFireActionReleased() => player.TryEndGrapplingHook();
    private void OnFlashlightAction() => player.TryUseFlashlight();
    private void OnInteracAction() => player.TryInteract();
}
