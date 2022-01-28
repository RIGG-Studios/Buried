using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPositioner : PlayerComponent
{
    [SerializeField]
    private float offset;
    [SerializeField]
    private Transform player;

    private void Update()
    {
        transform.position = player.position + playerMovement.GetMovementDirection() * offset;
    }
}
