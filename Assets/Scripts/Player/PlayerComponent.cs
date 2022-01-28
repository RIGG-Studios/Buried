using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{ 
    public PlayerMovement playerMovement { get; private set; }
    public PlayerCamera playerCamera { get; private set; }

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        playerCamera = FindObjectOfType<PlayerCamera>();
    }
}
