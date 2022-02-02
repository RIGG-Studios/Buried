using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputActions inputActions;

    PlayerMovement movement;
    GrapplingHook grappleHook;
    FlashlightPositioner flashlightPositioner;
    FlashlightManager flashLightManager;
    PlayerUI playerUI;
    PlayerInteractionManager playerInteraction;

    public RoomController currentRoom;

    private void Awake()
    {
        inputActions = new InputActions();

        inputActions.Player.Fire.started += ctx => OnLeftClick();
        inputActions.Player.Fire.canceled += ctx => OnLeftClickReleased();
        inputActions.Player.Flashlight.performed += ctx => UpdateFlashLight();
        inputActions.Player.Map.performed += ctx => ToggleMap();
        inputActions.Player.Crouch.performed += ctx => OnCrouchPressed();
        inputActions.Player.Interact.performed += ctx => OnInteractPressed();

        inputActions.Player.Enable();
    }

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        grappleHook = GetComponent<GrapplingHook>();
        flashlightPositioner = FindObjectOfType<FlashlightPositioner>();
        flashLightManager = GetComponent<FlashlightManager>();
        playerUI = GetComponent<PlayerUI>();
        playerInteraction = GetComponent<PlayerInteractionManager>();
    }

    private void Update()
    {
        Vector2 movementInput = inputActions.Player.Move.ReadValue<Vector2>();

        movement.UpdateInputVector(movementInput);
    }

    private void OnLeftClick()
    {
        if (grappleHook.TryStartGrapple())
            movement.enabled = false;
    }

    private void OnLeftClickReleased()
    {
        if (grappleHook.TryEndGrapple())
            movement.enabled = true;
    }

    private void UpdateFlashLight()
    {
        flashLightManager.ToggleFlashlight();
    }

    private void OnCrouchPressed()
    {
        movement.ToggleCrouch();
    }

    private void OnInteractPressed()
    {
        if (playerInteraction.hoveredObject != null) 
            playerInteraction.InteractWithObject();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Room")) 
        {
            RoomController roomProperties = collision.gameObject.GetComponent<RoomController>();

            if (roomProperties == null)
                return;

            if (roomProperties.useShadow)
            {
               if(currentRoom && currentRoom.useShadow) currentRoom.shadowRender.SetActive(true);
                roomProperties.shadowRender.SetActive(false);
            }

            currentRoom = roomProperties;

            EnterNewRoom(roomProperties.room);
        }
    }


    private void EnterNewRoom(Room room)
    {
        if (!room.breathable)
        {

        }

        if (room.restrictVision)
        {

        }

        if (room.tripOut)
        {

        }
    }
    private void ToggleMap() => playerUI.ToggleMap();

    public Vector3 GetMousePositionInWorldSpace() => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}

