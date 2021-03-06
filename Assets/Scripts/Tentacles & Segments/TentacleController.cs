using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(LineRenderer), typeof(TentacleStateManager))]
public class TentacleController : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;

    private LineRenderer line = null;
    private NavMeshAgent agent = null;
    private TentacleSpawner spawner = null;
    private AudioSource audioSource = null;
    private Player player = null;

    private List<Segment> segments = new List<Segment>();
    private Vector2[] segmentVelocity = new Vector2[0];

    public bool setup { get; private set; }
    public bool occupied { get; set; }
    public TentacleProperties properties { get; set; }
    public TentacleStateManager stateManager { get; private set; }


    [HideInInspector]
    public float targetSpeed;

    private WallLight[] wallLights;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        agent = GetComponentInChildren<NavMeshAgent>();
        stateManager = GetComponent<TentacleStateManager>();
        audioSource = GetComponentInChildren<AudioSource>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        wallLights = FindObjectsOfType<WallLight>();
    }

    public void InitializeTentacles()
    {
        if (properties == null)
            return;

        if (segments.Count > 0)
            segments.Clear();

        for (int i = 0; i < properties.tentacleSegments; i++)
        {
            float width = 25 - i;
            float length = 23 - i * 0.5f;
            float desiredAng = Mathf.Max(0, Mathf.Min(Mathf.PI * 0.1f, Mathf.Sin(i * 0.2f - 0.5f) * 0.4f));

            segments.Add(new Segment(i, properties, length, width, desiredAng));
        }

        targetSpeed = properties.tentacleMoveSpeed;
        segmentVelocity = new Vector2[segments.Count];
        line.positionCount = properties.tentacleSegments;
        setup = true;
    }
    

    public void UpdateSegmentPositions(Vector2 target)
    {
        if (spawner == null)
            return;

        segments[0].position = spawner.spawnPoint;

        for (int i = 1; i < segments.Count; i++)
        {
            Segment currentSeg = segments[i];
            Segment previousSeg = segments[i - 1];

            Vector2 segPos = currentSeg.UpdatePosition(previousSeg, target, wallLayer);

            segments[i].position = Vector2.SmoothDamp(segments[i].position, segPos, ref segmentVelocity[i], properties.tentacleMoveSpeed);    
        }

        audioSource.transform.position = segments[segments.Count - 1].position;

        line.SetPositions(GetSegmentPositions());
    }

    public void PlayTentacleSound()
    {
        AudioClip clip = properties.GetRandomAudio();

        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    public void CheckForLights()
    {
        if (wallLights == null || wallLights.Length <= 0)
            return;

        foreach(WallLight light in wallLights)
        {
            float dist = (light.transform.position - GetTentacleEndPoint()).magnitude;

            if(dist < 5)
            {
                light.minIntensity = Mathf.Lerp(light.minIntensity, 0f, Time.deltaTime * 5f);
                light.maxIntensity = Mathf.Lerp(light.maxIntensity, 0f, Time.deltaTime * 5f);
            }
            else
            {
                light.maxIntensity = 2f;
                light.maxIntensity = 5f;
            }
        }
    }

    public void UpdateAgentPosition(Vector3 position)
    {
        if (agent == null)
            return;

        agent.SetDestination(position);
    }

    public void SetAgentPosition(Vector3 position)
    {
        if (agent == null)
            return;

        agent.transform.position = position;
    }

    public void SetNewAnchor(TentacleSpawner spawner)
    {
        Vector3 anchor = spawner.spawnPoint;

        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].position = anchor;
            segments[i].origin = anchor + new Vector3(0, -1f, 0);
        }

        agent.enabled = false;
        agent.transform.position = anchor;
        agent.enabled = true;

        PlayTentacleSound();
        this.spawner = spawner;
    }

    public void ResetTentacle()
    {
        spawner.occupied = false;
        occupied = false;
        spawner = null;
        line.positionCount = 0;

        GameManager.instance.game.tentacleManager.ResetTentacle(this);
    }

    private Vector3[] GetSegmentPositions()
    {
        List<Vector3> seg = new List<Vector3>();

        for (int i = 0; i < segments.Count; i++)
        {
            seg.Add(segments[i].position);
        }

        return seg.ToArray();
    }

    public bool IsTentacleScared(float playerDistFromTentacle)
    {
        bool scared = false;

        if (playerDistFromTentacle <= properties.lightDistance)
        {
            if (player.flashLight.GetState() == FlashlightStates.On)
            {
                float ang = Vector2.Angle(Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition()), GetAgentPosition());

                if (ang < 5f)
                    scared = true;
            }
        }

        Flare[] flares = FindObjectsOfType<Flare>();

        if (flares.Length > 0)
        {
            foreach (Flare f in flares)
            {
                float dist = (f.transform.position - GetTentacleEndPoint()).magnitude;

                if (dist <= properties.lightDistance)
                    scared = true;
            }
        }

        return scared;
    }

    public Segment GetLastTentacle() 
    {
        return segments[segments.Count - 1];
    }

    public Vector3 GetTentacleEndPoint()
    {
      return  segments[segments.Count - 1].position;
    }

    public float GetDistanceBetweenAgentAndEndPoint()
    {
        return (agent.transform.position - GetTentacleEndPoint()).magnitude;
    }

    public float GetDistanceBetweenPlayerAndEndPoint()
    {
        return (GetTentacleEndPoint() - GameManager.instance.game.player.GetPosition()).magnitude;
    }

    public float GetTentacleDistance()
    {
        return (segments[0].position - segments[segments.Count-1].position).magnitude;
    }

    public TentacleProperties GetTentacleProperties()
    {
        return properties;
    }

    public Vector3 GetAnchorPosition()
    {
       return spawner.spawnPoint;
    }

    public Vector3 GetAgentPosition()
    {
       return agent.nextPosition;
    }
    public float GetDistanceBetweenPlayerAndAnchor()
    {
        return (spawner.spawnPoint - GameManager.instance.game.player.GetPosition()).magnitude;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }
}

