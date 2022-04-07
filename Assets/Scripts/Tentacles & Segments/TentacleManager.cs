using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// this class is the "monster". It controlls all of the tentacles, where they will spawn, what they are doing etc
/// </summary>
public class TentacleManager : MonoBehaviour
{
    public static TentacleManager instance;

    [SerializeField] private TentacleProperties[] tentacleProperties;
    [SerializeField] private TentacleDifficultyProgression progression;
    [SerializeField] private GameObject tentaclePrefab;

    public bool initialized { get; private set; }

    private List<TentacleController> tentacles = new List<TentacleController>();
    private List<TentacleController> attackingTentacles = new List<TentacleController>();
    private TentacleSpawner[] spawners;



    void Start()
    {
        instance = this;

        spawners = FindObjectsOfType<TentacleSpawner>();
    }

    /// <summary>
    /// when this object is enabled, subscribe to the OnGeneratorTurnedOn from the GameEvents
    /// </summary>

    private void OnEnable()
    {
        GameEvents.OnGeneratorTurnedOn += OnGeneratorTurnedOn;
    }

    /// <summary>
    /// when this object is disabled, unsubscribe to the OnGeneratorTurnedOn from the GameEvents
    /// </summary>
    private void OnDisable()
    {
        GameEvents.OnGeneratorTurnedOn -= OnGeneratorTurnedOn;
    }

    /// <summary>
    /// method used for setting up the monster, we don't want to do this in Start because the game doesn't start right away.
    /// </summary>
    public void Initialize()
    {
        //loop through all the tentacles, spawn them and initialize them
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

    /// <summary>
    /// method that gets called when the player turns on a generator, through the delegate void in GameEvents.
    /// </summary>
    /// <param name="genCount"></param>
    private void OnGeneratorTurnedOn(int genCount)
    {
        //role a random "dice" since we dont want to spawn the tentacle every time a generator is turned on, if so find the closest spawners based on the 
        //current difficulty from the DifficultyProgressionProperties. For example, if 3 generators are turned on, spawn more then 1 tentacle. Then we spawn a tentacle
        //using the SpawnTentacle method.
        int rng = Random.Range(1, 2);

        if (genCount <= 0 || rng == 2)
            return;

        DifficultyProgressionProperties difficultProps = progression.FindProperty(genCount);

        if (difficultProps != null)
        {
            TentacleSpawner[] spawners = FindClosestSpawners(difficultProps.tentaclesToSpawn, GameManager.instance.game.player.GetPosition());

            foreach (TentacleSpawner s in spawners)
                SpawnTentacle(s, 1);
        }
    }


    /// <summary>
    /// when we want to spawn a tentacle at a spawnpoint, we call this method. This is used for spawning tentacles at their spawnpoint
    /// </summary>
    /// <param name="spawner"></param>
    /// <param name="tentaclesToSpawn"></param>
    public void SpawnTentacle(TentacleSpawner spawner, int tentaclesToSpawn)
    {
        //check if the spawner already has a tentacle attached, or the player already has more then the max attacking tentacles
        //if so, dont spawn a tentacle. If its clear, we'll spawn a tentacle based on the amount given and set it up in the TentacleController .
        if (spawner.occupied || attackingTentacles.Count >= GameManager.instance.game.currentLevelProperties.maxAttackingTentacles)
            return;

        TentacleController[] tentacles = GetTentacles(tentaclesToSpawn);

        for (int i = 0; i < tentacles.Length; i++)
        {
            tentacles[i].gameObject.SetActive(true);
            tentacles[i].SetNewAnchor(spawner);
            tentacles[i].stateManager.TransitionStates(TentacleStates.Attack);
            attackingTentacles.Add(tentacles[i]);
        }

        spawner.occupied = true;
    }

    /// <summary>
    /// Resets a tentacle, called from a TentacleController when the tentacle is done, for example if it retreats back to the hole
    /// </summary>
    /// <param name="controller"></param>
    public void ResetTentacle(TentacleController controller)
    {
        if (controller == null || !attackingTentacles.Contains(controller))
            return;

        attackingTentacles.Remove(controller);
    }

    /// <summary>
    /// Method to find the closest spawners to the player, using linq for the cleanest results.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private TentacleSpawner[] FindClosestSpawners(int amount, Vector3 pos)
    {
        //create a new list and order it by the distance between each element of the spawners around the map, and the given position, usually the player position.
        //this is because we want to spawn tentacles close to the player, and not on the other side of the map.
        List<TentacleSpawner> spawners = this.spawners.OrderBy(x => (pos - x.transform.position).magnitude).ToList();
        IEnumerable<TentacleSpawner> s = spawners.Take(amount);

        return s.ToArray();
    }

    /// <summary>
    /// method for finding tentacles that are not currently attacking. Since we want the monster to have a set amount of tentacles, 
    /// we need to make sure we find ones that are not currently attacking the player
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
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

    /// <summary>
    /// used to find the closest tentacle to the player. Used by the paranoid system so we only calculate the
    /// distance between the closest one if multiple tentacles are attacking.
    /// </summary>
    /// <param name="playerPos"></param>
    /// <returns></returns>
    public TentacleController GetClosestTentacleToPlayer(Vector3 playerPos)
    {
        TentacleController tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (TentacleController t in attackingTentacles)
        {
            float dist = Vector3.Distance(t.GetTentacleEndPoint(), playerPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }

        return tMin;
    }
}