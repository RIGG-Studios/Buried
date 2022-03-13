using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    private Player player = null;
    private PostProcessingManager postProcessingManager = null;

    public Room currentRoom { get; private set; }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        postProcessingManager = FindObjectOfType<PostProcessingManager>();
    }
}
