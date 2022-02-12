using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  

public class TentacleAi : MonoBehaviour
{
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

        TransitionStates();

        agent.SetDestination(currentTarget.position);
    }

    public void TransitionStates()
    {
        float distanceBetweenPlayer = (transform.position - player.transform.position).magnitude;
        float distanceBetweenBody = (transform.position - body.position).magnitude;
        float angle = Vector3.Angle(transform.position, player.transform.position);

        if (distanceBetweenPlayer < properties.aiDistance && !player.isHiding)
        {
            currentTarget = target;
            targettingPlayer = true;
            line.rotateAngle = 2;
        }

        if(distanceBetweenBody >= properties.tentacleLength || player.isHiding)
        {
            currentTarget = body;
            targettingPlayer = false;
            line.rotateAngle = 0;
        }

        if (distanceBetweenPlayer <= properties.lightDistance && player.flashLightEnabled && targettingPlayer)
        {
            if (angle <= 90)
            {
                currentTarget = body;
                targettingPlayer = false;
            }
        }
    }

    public void ResetAI() => transform.position = body.position;
}
