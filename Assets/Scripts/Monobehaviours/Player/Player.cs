using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerMovement movement { get; private set; }
    public GrapplingHook grappleHook { get; private set; }
    public FlashlightPositioner flashlightPositioner { get; private set; }
    public FlashlightManager flashLightManager { get; private set; }
    public PlayerUI playerUI { get; private set; }
    public PlayerAnimator playerAnimator { get; private set; }
    public PlayerInteractionManager playerInteraction { get; private set; }

    public RoomController currentRoom;

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
    }

    public void TryUseGrapplingHook()
    {
        bool canGrapple = grappleHook.TryStartGrapple();

        if (canGrapple)
            movement.enabled = false;
    }

    public void TryEndGrapplingHook()
    {
        bool canEnd = grappleHook.TryEndGrapple();

        if (canEnd)
            movement.enabled = true;
    }

    public void TryInteract()
    {
        if(playerInteraction.hoveredObject)
            playerInteraction.InteractWithObject();
    }

    public void TryUseFlashlight()
    {
        flashLightManager.ToggleFlashlight();
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

    public Vector3 GetMovement() => movement.GetMovementDirection();
}

