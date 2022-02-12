using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleLineRenderer : MonoBehaviour
{
    public float hitOffset;
    public float rotateAngle;
    public TentacleProperties properties;
    public LayerMask wallLayer;

    TentacleAi agent;
    LineRenderer line;
    Transform body;
    Player player;
    bool grabPlayer;

    Vector3[] segments;
    Vector3[] segmentVelocity;

    private void Awake()
    {
        agent = GetComponentInChildren<TentacleAi>();
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        player = Game.instance.player;
    }

    public void SetupTentacle(Transform body)
    {
        line.positionCount = properties.tentacleSegments;
        segments = new Vector3[properties.tentacleSegments];
        segmentVelocity = new Vector3[properties.tentacleSegments];
        this.body = body;

        if (agent)
            agent.SetupAI(properties, true, body, this);
    }

    private void LateUpdate()
    {
        UpdateSegments();
    }

    private void UpdateSegments()
    {
        segments[0] = body.position;

        for (int i = 1; i < segments.Length; i++)
        {
            Vector3 origin =  segments[i - 1];
            Vector3 direction =  Quaternion.AngleAxis(((i > 1 ? i : 0) * rotateAngle), Vector3.forward) *
                (agent.transform.position - segments[i - 1]).normalized;

            Vector3 pos = origin + direction;

            if (i >= segments.Length / 2)
                direction = agent.transform.position - segments[i - 1];

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, direction.magnitude, wallLayer);

            if (hit.collider != null)
                pos = hit.point + (hit.normal * hitOffset);


            segments[i] = Vector3.SmoothDamp(segments[i], pos, ref segmentVelocity[i], properties.tentacleMoveSpeed);
        }

        float agentDist = (agent.transform.position - GetTentacleEndPoint()).magnitude;
        float playerDist = (GetTentacleEndPoint() - player.GetPosition()).magnitude;

        if (agentDist > properties.tentacleAIMaxDistance)
            agent.ResetAI();

        if(playerDist <= 1 && !player.flashLightEnabled)
        {
            if(!player.isHiding)
            {
                player.DoAction(PlayerActions.GrabByMonster);
                player.transform.position = GetTentacleEndPoint();
                agent.ResetAI();
                grabPlayer = true;
            }
        }

        if(grabPlayer && player.flashLightEnabled)
        {
            player.DoAction(PlayerActions.GrabByMonster);
            grabPlayer = false;
        }

        line.SetPositions(segments);
    }

    public Vector3 GetTentacleEndPoint() => segments[segments.Length - 1];
}

