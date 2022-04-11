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
        if (collision.gameObject.tag == "Player" && CanSpawnTentacle()) 
            TentacleManager.instance.SpawnTentacle(this, 1);
    }

    private bool CanSpawnTentacle()
    {
        bool spawn = false;
        int rng = Random.Range(1, 2);

        if (rng == 1)
            spawn = true;

        return spawn;
    }
}
