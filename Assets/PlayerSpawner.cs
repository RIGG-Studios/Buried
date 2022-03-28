using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSpawner 
{
    public static GameObject player
    {
        get
        {
            return (GameObject)Resources.Load("Player", typeof(GameObject));
        }
    }

    public static Player SpawnPlayer(Transform spawnPoint)
    {
        if (player == null)
            return null;

        Player playerObj = GameObject.Instantiate(player, spawnPoint.position, spawnPoint.rotation).GetComponent <Player>();

        return playerObj != null ? playerObj : null;
    }
}
