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

    private float lastSpawnTime;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && CanSpawnTentacle() && (Time.timeSinceLevelLoad - lastSpawnTime) > 5f)
        {
            TentacleManager.instance.SpawnTentacle(this, 1);
            lastSpawnTime = Time.timeSinceLevelLoad;
        }
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
