using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game instance;
    public MonsterController monster { get; private set; }  
    public Player player { get; private set; }


    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<Player>();
        monster = FindObjectOfType<MonsterController>();
    }

    public TentacleSpawner GetClosestSpawnerToPlayer()
    {
        TentacleSpawner tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (TentacleSpawner t in GetAllTentaclesSpawners())
        {
            float dist = Vector3.Distance(t.spawnPoint, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }

        return tMin;
    }

    public TentacleSpawner[] GetAllTentaclesSpawners()
    {
        TentacleSpawner[] spawners = FindObjectsOfType<TentacleSpawner>();

        return spawners.Length > 0 ? spawners : null;
    }
}
