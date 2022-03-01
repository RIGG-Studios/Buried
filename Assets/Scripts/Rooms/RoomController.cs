using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomController : MonoBehaviour
{
    public Room room;

    public GameObject[] blockedPieces;
    public GameObject[] entrances = new GameObject[2];

    public GameObject mapUIElement;
    public bool useShadow;
    public GameObject shadowRender;

    public void OnRoomEntered()
    {

    }
}


