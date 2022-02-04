using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0, 20)]
    public float movementSpeed;
    [Range(0, 10)]
    public float camOffset;
    [Range(0, 10)]
    public float rotateSpeed;

    public float minMoveDistance;
    public float rotationMinDistance;

    public bool canMove { get; private set; }

    public bool wasMoving { get; private set; }
    public bool canRotate { get; private set; }

    Vector2 movement;
    Vector3 mousePos;

    Rigidbody2D physics;
    Player player;
    PlayerCamera playerCam;


    private void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        playerCam = FindObjectOfType<PlayerCamera>();
        player = FindObjectOfType<Player>();
        playerCam.SetTarget(transform);
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition());
        canMove = (transform.position - mousePos).magnitude >= minMoveDistance;
        canRotate = (transform.position - mousePos).magnitude >= rotationMinDistance;


        if (!canMove && wasMoving)
        {
            player.playerInteraction.UpdateInteractionAssistRotation(transform.position - mousePos);
        }

        if (canMove)
            playerCam.SetOffset(transform.up * camOffset);

        wasMoving = canMove;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 physicsPos = physics.position;
            physics.MovePosition(physicsPos + (movement.x * transform.right + movement.y * transform.up) * movementSpeed * Time.fixedDeltaTime);
        }

        if (canRotate)
        {
            Vector2 dir = (mousePos - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, dir), Time.fixedDeltaTime * rotateSpeed);
        }
    }

    public Vector3 GetMovementDirection() => movement.normalized;
    public void UpdateInputVector(Vector2 input) => movement = input;
}
