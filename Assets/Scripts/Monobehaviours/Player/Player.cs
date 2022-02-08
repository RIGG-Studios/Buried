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

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        flashLightManager = GetComponentInChildren<FlashlightManager>();
        playerUI = GetComponent<PlayerUI>();
        playerInteraction = GetComponent<PlayerInteractionManager>();
        inventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        LookingAtEnemy();
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

    public bool LookingAtEnemy()
    {
        Vector3 mousePos = Utilites.GetMousePosition();
        Vector3 dir = (mousePos - transform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir,10, LayerMask.NameToLayer("Enemy"));
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);
        }
        return false;
    }
    public Vector3 GetMovement() => movement.GetMovementDirection();
}

