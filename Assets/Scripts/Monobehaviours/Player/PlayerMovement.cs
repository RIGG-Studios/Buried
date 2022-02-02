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

    Rigidbody2D physics;
    Vector2 movement;
    PlayerCamera playerCam;
    Animator animator;
    Player player;

    bool isCrouched;

    private void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        playerCam = FindObjectOfType<PlayerCamera>();
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();

        playerCam.SetTarget(transform);
    }

    private void Update()
    {
        playerCam.SetOffset(movement != Vector2.zero ? GetMovementDirection() * camOffset : Vector3.zero);

        Vector3 mousePos = player.GetMousePositionInWorldSpace();
    }

    private void FixedUpdate()
    {
        physics.MovePosition(physics.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    public void ToggleCrouch()
    {
        isCrouched = !isCrouched;
    }
    public Vector3 GetMovementDirection() => movement.normalized;
    public void UpdateInputVector(Vector2 input) => movement = input;
}
