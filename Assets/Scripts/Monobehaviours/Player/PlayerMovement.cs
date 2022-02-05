using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float camOffset;

    public float rotateSpeed;
    public float rotationDistanceMultiplier;
    public float rotationMinDistance;

    public bool canMove;

    public bool wasMoving { get; private set; }
    public bool canRotate { get; private set; }

    Vector2 movement;
    Vector3 mousePos;

    Rigidbody2D physics;
    Player player;
    PlayerCamera playerCam;

    float mouseDist;


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
        canRotate = (transform.position - mousePos).magnitude >= rotationMinDistance;

        if (!canRotate)
            rotateSpeed = 0.001f;
        else
            rotateSpeed = 5;

        if (!canMove && wasMoving)
        {
            player.playerInteraction.UpdateInteractionAssistRotation(transform.position - mousePos);
        }

        playerCam.SetOffset(transform.up * camOffset);

        wasMoving = canMove;
    }

    private void FixedUpdate()
    {
        Vector3 physicsPos = physics.position;
        physics.MovePosition(physicsPos + (movement.x * transform.right + movement.y * transform.up) * movementSpeed * Time.fixedDeltaTime);

        Vector2 dir = (mousePos - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(Vector3.forward, dir), rotateSpeed);
    }

    public Vector3 GetMovementDirection() => movement.normalized;
    public void UpdateInputVector(Vector2 input) => movement = input;
}
