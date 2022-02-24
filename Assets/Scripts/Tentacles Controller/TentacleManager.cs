using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleManager : MonoBehaviour
{
    public bool initialized { get; private set; }

    [SerializeField, Range(0, 10)]
    private int maxTentacles = 5;

    [SerializeField]
    private GameObject tentaclePrefab;

    public static TentacleManager instance;

    private List<TentacleData> tentacles = new List<TentacleData>();

    void Start()
    {
        instance = this;

        SetupTentacles();
    }

    public void SetupTentacles()
    {
        for(int i = 0; i < maxTentacles; i++)
        {
            TentacleController controller = Instantiate(tentaclePrefab, Vector2.zero, Quaternion.identity).GetComponent<TentacleController>();

            if(controller != null)
            {
                TentacleData data = new TentacleData(controller);

                controller.stateManager.TransitionStates(TentacleStates.Idle);
                controller.gameObject.name = "Tentacle" + i;
                controller.gameObject.SetActive(false);
                controller.transform.parent = transform;

                tentacles.Add(data);
            }
        }

        initialized = true;
    }

    public void SpawnTentacle(TentacleSpawner spawner, int tentaclesToSpawn)
    {
        Debug.Log("spawn");
        TentacleData[] tentacles = GetTentacles(tentaclesToSpawn);

        for (int i = 0; i < tentacles.Length; i++)
        {
            tentacles[i].controller.gameObject.SetActive(true);
            tentacles[i].controller.SetNewAnchor(spawner);
            tentacles[i].controller.stateManager.TransitionStates(TentacleStates.Attack);
        }

        spawner.occupied = true;
    }

    private TentacleData[] GetTentacles(int amount)
    {
        List<TentacleData> data = new List<TentacleData>();

        for (int z = 0; z < amount; z++)
        {
            for(int i = 0; i < tentacles.Count; i++)
            {
                if (!tentacles[i].IsOccupied())
                {
                    if(data.Count >= amount)
                        break;

                    data.Add(tentacles[i]);
                }
            }
        }

        return data.ToArray();
    }

}

public class TentacleData
{
    public TentacleController controller { get; set; }
    public TentacleData(TentacleController controller)
    {
        this.controller = controller;
    }
    public bool IsOccupied() => controller.occupied;
}