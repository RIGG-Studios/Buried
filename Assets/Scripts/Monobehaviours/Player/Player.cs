using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerActions
{
    ToggleFlashlight,
    GrapplingHook,
    EndGrapplingHook,
    Interact,
    Hide,
    UnHide
}

public class Player : MonoBehaviour
{
    public Transform mouseRotate;
    public bool isHiding { get; private set; }
   [HideInInspector] public bool flashLightEnabled;

    public PlayerMovement movement { get; private set; }
    public FlashlightManager flashLightManager { get; private set; }
    public PlayerUI playerUI { get; private set; }
    public PlayerInteractionManager playerInteraction { get; private set; }
    public Inventory inventory { get; private set; }

    public RoomController currentRoom;

    Transform trans;
    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        flashLightManager = GetComponentInChildren<FlashlightManager>();
        playerUI = GetComponent<PlayerUI>();
        playerInteraction = GetComponent<PlayerInteractionManager>();
        inventory = FindObjectOfType<Inventory>();

        trans = transform;
    }

    public void DoAction(PlayerActions action)
    {
        switch (action)
        {
            case PlayerActions.Interact:
                if (playerInteraction.hoveredObject) playerInteraction.InteractWithObject();
                break;

            case PlayerActions.EndGrapplingHook:

                break;

            case PlayerActions.GrapplingHook:

                break;

            case PlayerActions.ToggleFlashlight:
                if (inventory.HasItem(Item.WeaponTypes.Flashlight)) flashLightManager.ToggleFlashlight(out flashLightEnabled);
                break;

            case PlayerActions.Hide:
                ToggleHide();
                break;

            case PlayerActions.UnHide:
                ToggleHide();
                break;
        }
    }

    private void ToggleHide()
    {
        if (!isHiding)
        {
            movement.enabled = false;
            movement.sprite.enabled = false;
            isHiding = true;
        }
        else
        {
            movement.enabled = true;
            movement.sprite.enabled = true;
            isHiding = false;
        }
    }

    public void GrabPlayer()
    {
        movement.enabled = false;
        GetComponent<Collider2D>().enabled = false;
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

    public Vector3 GetPosition() => trans.position;
}

