using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerComponent
{
    [SerializeField, Range(0, 20)]
    private float movementSpeed;

    float horizontal;
    float vertical;
    Rigidbody2D physics;
    Vector2 movement;


    private void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        playerCamera.SetTarget(transform);
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        movement = new Vector2(horizontal, vertical);

        if(movement != Vector2.zero)
        {
            playerCamera.SetOffset(GetMovementDirection() * 2f);
        }
        physics.MovePosition(physics.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        physics.MovePosition(physics.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    public Vector3 GetMovementDirection() => movement.normalized;
}
