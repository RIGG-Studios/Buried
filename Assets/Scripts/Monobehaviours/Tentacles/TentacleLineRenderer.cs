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
    Vector2[] segmentVelocity;
    bool setup;

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
        segmentVelocity = new Vector2[properties.tentacleSegments];
        this.body = body;

        if (agent)
            agent.SetupAI(properties, true, body, this);

        setup = true;
    }

    private void Update()
    {
        if (!setup)
            return;

        UpdateSegments();

        float lengthOfTentacle = (segments[segments.Length - 1] - segments[0]).magnitude;
    }

    private void UpdateSegments()
    {
        segments[0] = body.position;
        for (int i = 1; i < segments.Length; i++)
        {
            Vector2 origin = segments[i - 1];
            Quaternion rotation = Quaternion.AngleAxis(i * 2f, Vector3.forward);
            Vector2 direction = rotation*  (agent.transform.position - segments[i - 1]).normalized;
            Vector2 pos = origin + direction.normalized;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, direction.magnitude, wallLayer);
            Debug.DrawRay(origin, direction, Color.red);


            if (hit.collider != null)
                pos = hit.point;

            segments[i] = Vector2.SmoothDamp(segments[i], pos, ref segmentVelocity[i], properties.tentacleMoveSpeed);
        }

        line.SetPositions(segments);
    }

    public Vector3 GetTentacleEndPoint() => segments[segments.Length - 1];
    private float GetDistanceBetweenAgentAndEndPoint() => (agent.transform.position - GetTentacleEndPoint()).magnitude;
    private float GetDistanceBetweenPlayerAndEndPoint() => (GetTentacleEndPoint() - player.GetPosition()).magnitude;
    private float GetDistanceBetweenPlayerAndBody() => (body.position - GetTentacleEndPoint()).magnitude;
}

