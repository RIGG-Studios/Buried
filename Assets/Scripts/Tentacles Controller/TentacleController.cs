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

    LineRenderer line;
    NavMeshAgent agent;

    Vector3[] segments;
    Vector2[] segmentVelocity;

    TentacleSpawner spawner;
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
        line.positionCount = properties.tentacleSegments;
        segments = new Vector3[properties.tentacleSegments];
        segmentVelocity = new Vector2[properties.tentacleSegments];
        setup = true;
    }

    public void UpdateSegments(Vector3 targetDir)
    {
        segments[0] = spawner.spawnPoint;

        for (int i = 1; i < segments.Length; i++)
        {
            Vector2 origin = segments[i - 1];
            Quaternion rotation = Quaternion.AngleAxis(i * tentacleRotation, Vector3.forward);
            Vector2 direction = rotation * (targetDir - segments[i - 1]).normalized;
            Vector2 pos = origin + direction.normalized;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, direction.magnitude, wallLayer);

            if (hit.collider != null)
                pos = hit.point;

            segments[i] = Vector2.SmoothDamp(segments[i], pos, ref segmentVelocity[i], properties.tentacleMoveSpeed);
        }

        line.SetPositions(segments);
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

        for (int i = 0; i < segments.Length; i++)
        {
            segments[i] = anchor;
            segmentVelocity[i] = anchor;
        }

        this.spawner = spawner;
    }

    public void SetSegmentPositions(Vector3 position)
    {
        for(int i = 0; i < segments.Length; i++)
        {
            segments[i] = position;
            segmentVelocity[i] = position;
        }

        line.SetPositions(segments);
    }

    public void ResetTentacle()
    {
        spawner.occupied = false;
        spawner = null;

        segments = new Vector3[0];
        segmentVelocity = new Vector2[0];
        line.positionCount = segments.Length;
        line.SetPositions(segments);
    }

    public Vector3 GetTentacleEndPoint() => segments[segments.Length - 1];
    public float GetDistanceBetweenAgentAndEndPoint() => (agent.transform.position - GetTentacleEndPoint()).magnitude;
    public float GetDistanceBetweenPlayerAndEndPoint() => (GetTentacleEndPoint() - Game.instance.player.GetPosition()).magnitude;
    public float GetDistanceBetweenEndPointAndHole() => (spawner.spawnPoint - GetTentacleEndPoint()).magnitude;
    public TentacleProperties GetTentacleProperties() => properties;
    public Vector3 GetAnchorPosition() => spawner.spawnPoint;
    public Vector3 GetAgentPosition() => agent.nextPosition;
}

