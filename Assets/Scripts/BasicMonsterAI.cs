using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterAI : MonoBehaviour
{
    [Range(0, 30)] public float targetPlayerDist;
    [Range(0, 30)] public float lightDist;
    [Range(0, 30)] public float minDist;
    public float speed;
    public Transform[] wayPoints;

    private Vector3 velocity;
    private bool wasTargettingPlayer;

    private int currentWayPointIndex = -1;
    Transform currentWayPoint = null;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentWayPoint = GetNextWayPoint();
    }
    private void Update()
    {
        Vector3 playerPos = Utilites.GetPlayerPosition();
        bool moveTowards = CanTargetPlayer(playerPos);

        //if were tracking the player last frame and we are not tracking the player this frame, reset the poistion to the next wayPoint
        if ((wasTargettingPlayer && !moveTowards))
        {
            currentWayPoint = GetNextWayPoint();
        }
        else if (moveTowards)
        {
            RotateTowardsDir(playerPos - transform.position);
            currentWayPoint = Utilites.GetPlayer().transform;
        }

        float dist = (transform.position - currentWayPoint.position).magnitude;

        if (dist <= minDist)
            currentWayPoint = GetNextWayPoint();


        float distanceBetweenPlayer = (transform.position - playerPos).magnitude;
        if (distanceBetweenPlayer <= lightDist && Utilites.GetPlayer().flashLightEnabled)
        {
            float angle = Quaternion.Angle(transform.rotation, Utilites.GetPlayer().mouseRotate.rotation);

            if(angle <= 30)
            {
                currentWayPoint = GetFurthestWayPointFromPlayer(playerPos);
            }
        }

        wasTargettingPlayer = moveTowards;
    }

    private void FixedUpdate()
    {
        Vector3 rbPos = rb.position;
        rb.MovePosition(rbPos + (currentWayPoint.position - transform.position).normalized * speed * Time.fixedDeltaTime);
    }

    public void RotateTowardsDir(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private Transform GetNextWayPoint()
    {
        currentWayPointIndex++;

        if (currentWayPointIndex >= wayPoints.Length)
            currentWayPointIndex = 0;


        Debug.Log(currentWayPointIndex);
        return wayPoints[currentWayPointIndex];
    }

    private bool CanTargetPlayer(Vector3 playerPos)
    {
        return (transform.position - playerPos).magnitude <= targetPlayerDist && !Utilites.GetPlayer().isHiding;
    }

    private Transform GetFurthestWayPointFromPlayer(Vector3 playerPos)
    {
        float furthestDist = 0;
        Transform furthestObj = null;

        foreach (Transform obj in wayPoints)
        {
            float dist = Vector3.Distance(obj.position, playerPos);
            if (dist > furthestDist)
            {
                furthestObj = obj;
                furthestDist = dist;
            }
        }
        return furthestObj;
    }
}
