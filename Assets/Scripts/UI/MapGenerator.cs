using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform mapParent;
    public void SetupMap(RoomController[] rooms)
    {
        /*/
        Transform[] mapParts = GetComponentsInChildren<Transform>();
        Debug.Log(mapParts);
        for(int i = 0; i < mapParts.Length; i++)
        {
            for(int z = 0; z < rooms.Length; z++)
            {
                if(rooms[z].mapUIElement == mapParts[i].gameObject)
                {
                    if (rooms[z].room.roomType == Room.RoomType.SecretRoom)
                        mapParts[i].gameObject.SetActive(false);
                }
            }
        }
        /*/
    }
}
