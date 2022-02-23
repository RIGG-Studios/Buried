using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer), typeof(TentacleStateManager))]
public class TentacleController : MonoBehaviour
{
    public bool setup { get; private set; }
    public bool occupied { get; set; }
    public TentacleStateManager stateManager { get; private set; }

    [SerializeField] private TentacleProperties properties;
    [SerializeField] private LayerMask wallLayer;

    public float frequency;

    LineRenderer line;
    NavMeshAgent agent;
    TentacleSpawner spawner;

    public List<Segment> segments = new List<Segment>();
    public List<Segment> queuedSegments = new List<Segment>();
    Vector2[] segmentVelocity;

    float tentacleRotation;
    float lastSegmentCountDist;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        agent = GetComponentInChildren<NavMeshAgent>();
        stateManager = GetComponent<TentacleStateManager>();
        tentacleRotation = 1f;
    }

    public void InitializeTentacles()
    {
        for (int i = 0; i < properties.tentacleSegments; i++)
        {
            segments.Add(new Segment(i, 2, 2, frequency, properties));
        }

        segmentVelocity = new Vector2[segments.Count];
        line.positionCount = properties.tentacleSegments;

        setup = true;
    }

    int segmentCount;
    public void UpdateSegments(Vector3 targetDir)
    {
        float dist = (Game.instance.player.GetPosition() - spawner.spawnPoint).magnitude;
        segmentCount = (int)(dist / properties.lengthBetweenSegments) * 5;

        segments[0].position = spawner.spawnPoint;
        line.positionCount = segmentCount;

        for (int i  = 1; i < segments.Count; i++)
        {
            if(i >= segmentCount)
            {
                queuedSegments.Add(segments[i]);
                segments.Remove(segments[i]);
                continue;
            }

            Segment currentSeg = segments[i];
            Segment previousSeg = segments[i - 1];

            Vector2 segPos = currentSeg.UpdatePosition(previousSeg, targetDir);

            segments[i].position = Vector2.SmoothDamp(segments[i].position, segPos, ref segmentVelocity[i], properties.tentacleMoveSpeed);
        
        }

        line.SetPositions(GetSegmentPositions());
    }

    public void UpdateQueuedSegments(bool state)
    {
        if (state) StartCoroutine(IELoopUpdateQueuedObjects());
        else StopCoroutine(IELoopUpdateQueuedObjects());
    }

    public IEnumerator IELoopUpdateQueuedObjects()
    {
        yield return new WaitForSeconds(1f);

        if (segments.Count != segmentCount)
        {
            for (int i = 0; i < queuedSegments.Count; i++)
            {
                segments.Add(queuedSegments[i]);
                queuedSegments.Remove(queuedSegments[i]);
            }
        }

        StartCoroutine(IELoopUpdateQueuedObjects());
    }

    public Vector3[] GetSegmentPositions()
    {
        List<Vector3> seg = new List<Vector3>();


        for(int i = 0; i < segments.Count; i++)
        {
            seg.Add(segments[i].position);
        }

        return seg.ToArray();
    }

    public void UpdateTentacleRotation(float targetRotation, float incraseSpeed)
    {
        tentacleRotation = Mathf.Lerp(tentacleRotation, targetRotation, Time.deltaTime * incraseSpeed);
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

    public void SetSegmentPositions(Vector3 position)
    {
        /*/
        for(int i = 0; i < segments.Count; i++)
        {
            segments[i] = position;
            segmentVelocity[i] = position;
        }

        line.SetPositions(segments);
        /*/
    }

    public void ResetTentacle()
    {
        spawner.occupied = false;
        spawner = null;

        segments.Clear();
        segmentVelocity = new Vector2[0];
        line.positionCount = segments.Count;
        line.SetPositions(new Vector3[0]);
    }

    public Vector3 GetTentacleEndPoint() => segments[segments.Count - 1].position;
    public float GetDistanceBetweenAgentAndEndPoint() => (agent.transform.position - GetTentacleEndPoint()).magnitude;
    public float GetDistanceBetweenPlayerAndEndPoint() => (GetTentacleEndPoint() - Game.instance.player.GetPosition()).magnitude;
    public float GetDistanceBetweenEndPointAndHole() => (spawner.spawnPoint - GetTentacleEndPoint()).magnitude;
    public TentacleProperties GetTentacleProperties() => properties;
    public Vector3 GetAnchorPosition() => spawner.spawnPoint;
    public Vector3 GetAgentPosition() => agent.nextPosition;
}

