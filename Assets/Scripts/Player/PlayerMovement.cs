using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerComponent
{
    [SerializeField, Range(0, 20)]
    private float movementSpeed;
    [SerializeField, Range(0, 10)]
    private float offset;
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

        playerCamera.SetOffset(movement != Vector2.zero ? GetMovementDirection() * offset : Vector3.zero);

    }

    private void FixedUpdate()
    {
        physics.MovePosition(physics.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    public Vector3 GetMovementDirection() => movement.normalized;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        physics.gravityScale = 1;

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        physics.gravityScale = 0;
    }
}
