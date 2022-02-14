using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float camOffset;

    public float rotationMinDistance;
    public SpriteRenderer sprite;

    Vector2 movement;

    Rigidbody2D physics;
    PlayerCamera playerCam;
    Player player;


    private void Start()
    {
        physics = GetComponent<Rigidbody2D>();
        playerCam = FindObjectOfType<PlayerCamera>();
        player = GetComponent<Player>();
        playerCam.SetTarget(transform);
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition());
        Vector2 dir = (mousePos - transform.position).normalized;
        Vector3 camOffset = (dir * this.camOffset) + (movement * this.camOffset / 4);

        playerCam.SetOffset(player.isHiding ? Vector3.zero : camOffset);
    }

    private void FixedUpdate()
    {
        physics.MovePosition(physics.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
    public void UpdateInputVector(Vector2 input)
    {
        if(input.y != 0.0f)
        {
            sprite.flipX = false;
            sprite.transform.localRotation = Quaternion.Euler(0, 0, input.y > 0 ? 90f : -90f);
        }

        if(input.x != 0.0f)
        {
            sprite.flipX = input.x > 0.0f ? false : true;
            sprite.transform.localRotation = Quaternion.Euler(0, 0, 0f);
        }

        movement = input;
    }

    public Vector3 GetMovementDirection() => movement.normalized;
}

