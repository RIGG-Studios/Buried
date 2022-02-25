using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(LineRenderer),
    typeof(TentacleStateManager))]
public class TentacleController : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;

    private LineRenderer line = null;
    private NavMeshAgent agent = null;
    private TentacleSpawner spawner = null;
    private List<Segment> segments = new List<Segment>();
    private List<Segment> queuedSegments = new List<Segment>();
    private List<Vector3> agentTrackedPositions = new List<Vector3>();
    private Vector2[] segmentVelocity = new Vector2[0];
    private int segmentCount = 0;

    public bool setup { get; private set; }
    public bool occupied { get; set; }
    public float targetRotation { get; set; }
    public TentacleProperties properties { get; set; }
    public TentacleStateManager stateManager { get; private set; }

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        agent = GetComponentInChildren<NavMeshAgent>();
        stateManager = GetComponent<TentacleStateManager>();
    }

    public void InitializeTentacles()
    {
        if (properties == null)
            return;

        if (segments.Count > 0)
            segments.Clear();

        if (queuedSegments.Count > 0)
            queuedSegments.Clear();

        if (agentTrackedPositions.Count > 0)
            agentTrackedPositions.Clear();

        for (int i = 0; i < properties.tentacleSegments; i++)
        {
            segments.Add(new Segment(i, 2, 2, properties));
        }

        segmentVelocity = new Vector2[segments.Count];
        line.positionCount = properties.tentacleSegments;
        setup = true;
    }

    public void UpdateSegmentCount()
    {
        float dist = (Game.instance.player.GetPosition() - spawner.spawnPoint).magnitude;
        segmentCount = (int)(dist / properties.lengthBetweenSegments);
        segmentCount = Mathf.Clamp(segmentCount, 0, properties.tentacleSegments);
    }

    public void UpdateSegmentPositions(Vector3 dir)
    {
        segments[0].position = spawner.spawnPoint;
        for (int i = 1; i < segments.Count; i++)
        {
            if (i >= segmentCount)
            {
                segments[i].position = segments[i].UpdatePosition(segments[i - 1], dir != Vector3.zero ? dir : GetTrackedPosition(i), wallLayer, targetRotation);

                queuedSegments.Add(segments[i]);
                segments.Remove(segments[i]);
                continue;
            }

            Segment currentSeg = segments[i];
            Segment previousSeg = segments[i - 1];

            Vector2 segPos = currentSeg.UpdatePosition(previousSeg,  dir != Vector3.zero ? dir : GetTrackedPosition(i), wallLayer, targetRotation);

            segments[i].position = Vector2.SmoothDamp(segments[i].position, segPos, ref segmentVelocity[i], properties.tentacleMoveSpeed);    
        }

        line.positionCount = segmentCount;
        line.SetPositions(GetSegmentPositions());
    }

    public void UpdateAgentTrackedPositions()
    {
        bool canAdd = agentTrackedPositions.Count < segmentCount;

        if(canAdd && !agentTrackedPositions.Contains(agent.transform.position))
        {
            agentTrackedPositions.Add(agent.transform.position);
        }
    }
    public void UpdateQueuedSegments()
    {
        if (segments.Count != segmentCount)
        {
            for (int i = 0; i < queuedSegments.Count; i++)
            {
                segments.Add(queuedSegments[i]);
                queuedSegments.Remove(queuedSegments[i]);
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
        }

        this.spawner = spawner;
    }

    public void ResetTentacle()
    {
        spawner.occupied = false;
        occupied = false;
        spawner = null;
        line.positionCount = 0;
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

    public Vector3 GetTrackedPosition(int i)
    {
        if (i >= agentTrackedPositions.Count - 1)
        {
            if (agentTrackedPositions.Count <= 0)
                return agent.nextPosition;

            Vector3 pos = agentTrackedPositions[agentTrackedPositions.Count - 1];
            return pos;
        }

        Vector3 agentPos = agentTrackedPositions[i];
        agentTrackedPositions.Remove(agentPos);
        return agentPos;
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
        return (GetTentacleEndPoint() - Game.instance.player.GetPosition()).magnitude;
    }
    public float GetDistanceBetweenEndPointAndHole()
    {
       return (spawner.spawnPoint - GetTentacleEndPoint()).magnitude;
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
}

