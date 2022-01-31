using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 20)]
    private float movementSpeed;
    [SerializeField, Range(0, 10)]
    private float camOffset;


    float horizontal;
    float vertical;
    Rigidbody2D physics;
    Vector2 movement;
    PlayerCamera playerCam;

    private void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        playerCam = FindObjectOfType<PlayerCamera>();

        playerCam.SetTarget(transform);
    }

    private void Update()
    {
        playerCam.SetOffset(movement != Vector2.zero ? GetMovementDirection() * camOffset : Vector3.zero);
    }

    private void FixedUpdate()
    {
        physics.MovePosition(physics.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    public Vector3 GetMovementDirection() => movement.normalized;
    public void UpdateInputVector(Vector2 input) => movement = input;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        physics.gravityScale = 1;

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        physics.gravityScale = 0;
    }
}
