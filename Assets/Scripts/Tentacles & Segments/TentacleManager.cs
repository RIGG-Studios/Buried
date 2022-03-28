using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TentacleManager : MonoBehaviour
{
    public static TentacleManager instance;

    [SerializeField] private TentacleProperties[] tentacleProperties;
    [SerializeField] private TentacleDifficultyProgression progression;
    [SerializeField] private GameObject tentaclePrefab;

    public bool initialized { get; private set; }

    private List<TentacleController> tentacles = new List<TentacleController>();
    private TentacleSpawner[] spawners;

    void Start()
    {
        instance = this;

        spawners = FindObjectsOfType<TentacleSpawner>();
    }

    private void OnEnable()
    {
        GameEvents.OnNotePickedUp += OnNotePickedUp;
    }

    private void OnDisable()
    {
        GameEvents.OnNotePickedUp -= OnNotePickedUp;
    }


    private void OnNotePickedUp(int noteCount)
    {
        if (noteCount <= 0)
            return;

        DifficultyProgressionProperties difficultProps = progression.FindProperty(noteCount);

        if(difficultProps != null)
        {
            TentacleSpawner[] spawners = FindClosestSpawners(difficultProps.tentaclesToSpawn, Game.instance.player.GetPosition());

            foreach (TentacleSpawner s in spawners)
                SpawnTentacle(s, 1);
        }
    }

    public void Initialize()
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
                controller.stateManager.InitializeStates();

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

    private TentacleSpawner[] FindClosestSpawners(int amount, Vector3 pos)
    {
        List<TentacleSpawner> spawners = this.spawners.OrderBy(x => (pos - x.transform.position).magnitude).ToList();
        IEnumerable<TentacleSpawner> s = spawners.Take(amount);

        return s.ToArray();
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