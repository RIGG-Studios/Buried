using UnityEngine;

public class TentacleSpawner : MonoBehaviour
{
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
        if (collision.gameObject.tag == "Player")
            TentacleManager.instance.SpawnTentacle(this, 1);
    }
}
