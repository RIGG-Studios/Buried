using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPositioner : MonoBehaviour
{
    [SerializeField]
    private float offset;
    [SerializeField]
    private Transform player;

    public PlayerMovement playerMovement;

    private void Update()
    {
        transform.position = player.position + playerMovement.GetMovementDirection() * offset;
    }
}
