using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoleSize
{
    Small,
    Medium,
    Large
}
public class TentacleSpawner : MonoBehaviour
{
    [SerializeField]
    private HoleSize holeSize;
    [Range(0, 20)]
    public float holeRange;

    public Vector3 spawnPoint
    {
        get
        {
            return transform.position;
        }
    }

    public bool occupied { get; set; }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("player ent");
        if (collision.gameObject.tag == "Player")
            TentacleManager.instance.SpawnTentacle(this, 1);
    }
}
