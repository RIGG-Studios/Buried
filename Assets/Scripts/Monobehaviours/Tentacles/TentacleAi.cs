using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  

public class TentacleAi : MonoBehaviour
{
    public int areaMask;
    public int maxDistance;

    public Transform target;

    NavMeshAgent agent;
    TentacleProperties properties;
    Transform body;
    TentacleLineRenderer line;
    Player player;

    Transform currentTarget;
    bool targettingPlayer;
    bool setup;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentTarget = target;
    }

    private void Start()
    {
        player = Game.instance.player;
    }

    public void SetupAI(TentacleProperties properties, bool activateAI, Transform body, TentacleLineRenderer line)
    {
        this.body = body;
        this.properties = properties;
        this.line = line;

        agent.enabled = activateAI;
        setup = true;
    }

    private void Update()
    {
        if (!setup)
            return;

        NavMeshHit hit = GetNextPosition();
        if (hit.hit)
        {

        }

        agent.SetDestination(currentTarget.position);
    }

    public void TransitionStates()
    {

    }

    public void ResetAI() => transform.position = body.position;

    private NavMeshHit GetNextPosition()
    {
        NavMeshHit hit;
        bool foundPos =  agent.SamplePathPosition(areaMask, maxDistance, out hit);
        return hit;
    }
}
