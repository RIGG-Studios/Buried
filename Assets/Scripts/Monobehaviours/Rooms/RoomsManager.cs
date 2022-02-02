using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    public Player player;
    public List<RoomController> roomsInScene = new List<RoomController>();
    public float enemySpawnDistance;

    MapGenerator mapGen;

    private void Awake()
    {
        mapGen = FindObjectOfType<MapGenerator>();

        if(roomsInScene.Count <= 0)
        {
            RoomController[] roomsInScene = FindObjectsOfType<RoomController>() as RoomController[];

            if(roomsInScene.Length > 0)
            {
                for (int i = 0; i < roomsInScene.Length; i++)
                    this.roomsInScene.Add(roomsInScene[i]);
            }
        }

    //    mapGen.SetupMap(roomsInScene.ToArray());
    }

    public void Update()
    {
        for(int i = 0; i < roomsInScene.Count; i++)
        {
            if (!roomsInScene[i].room.spawnEnemies)
                continue;

            float distance = (player.transform.position - transform.position).magnitude;

            if(distance >= enemySpawnDistance)
            {
                SpawnEnemies(roomsInScene[i]);
            }    
        }
    }

    private void SpawnEnemies(RoomController room)
    {

    }
}
