using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float camOffset;

    public float rotationMinDistance;
    public Sprite sideWaysSprite;
    public Sprite forwardSprite;

    SpriteRenderer sprite;
    Animator animator;
    Vector2 movement;
    Rigidbody2D physics;
    PlayerCamera playerCam;
    Player player;


    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        physics = GetComponent<Rigidbody2D>();
        playerCam = FindObjectOfType<PlayerCamera>();
        player = GetComponent<Player>();
        playerCam.SetTarget(transform);
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition());
        Vector2 dir = (mousePos - transform.position).normalized;
        Vector3 camOffset = (dir * this.camOffset);

        playerCam.SetOffset(player.isHiding ? Vector3.zero : camOffset);
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
    }

    private void FixedUpdate()
    {
        physics.MovePosition(physics.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
    public void UpdateInputVector(Vector2 input)
    {

        movement = input;
    }

    public Vector3 GetMovementDirection() => movement.normalized;
}

