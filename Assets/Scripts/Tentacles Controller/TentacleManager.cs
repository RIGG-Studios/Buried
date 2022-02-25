using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleManager : MonoBehaviour
{
    public bool initialized { get; private set; }

    [SerializeField]
    private TentacleProperties[] tentacleProperties;

    [SerializeField]
    private GameObject tentaclePrefab;

    public static TentacleManager instance;

    private List<TentacleController> tentacles = new List<TentacleController>();

    void Start()
    {
        instance = this;

        SetupTentacles();
    }

    public void SetupTentacles()
    {
        for(int i = 0; i < tentacleProperties.Length; i++)
        {
            TentacleController controller = Instantiate(tentaclePrefab, Vector2.zero, Quaternion.identity).GetComponent<TentacleController>();

            if(controller != null)
            {
                controller.stateManager.TransitionStates(TentacleStates.Idle);
                controller.gameObject.name = "Tentacle" + i;
                controller.gameObject.SetActive(false);
                controller.transform.parent = transform;
                controller.properties = tentacleProperties[i];
                controller.InitializeTentacles();

                tentacles.Add(controller);
            }
        }

        initialized = true;
    }

    public void SpawnTentacle(TentacleSpawner spawner, int tentaclesToSpawn)
    {
        TentacleController[] tentacles = GetTentacles(tentaclesToSpawn);

        for (int i = 0; i < tentacles.Length; i++)
        {
            tentacles[i].gameObject.SetActive(true);
            tentacles[i].SetNewAnchor(spawner);
            tentacles[i].stateManager.TransitionStates(TentacleStates.Attack);
        }

        spawner.occupied = true;
    }

    private TentacleController[] GetTentacles(int amount)
    {
        List<TentacleController> data = new List<TentacleController>();

        for (int z = 0; z < amount; z++)
        {
            for(int i = 0; i < tentacles.Count; i++)
            {
                if (!tentacles[i].occupied)
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