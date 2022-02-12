using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    List<TentacleLineRenderer> tentacles = new List<TentacleLineRenderer>();

    private void Start()
    {
        TentacleLineRenderer[] tentacles = GetComponentsInChildren<TentacleLineRenderer>();

        if(tentacles.Length > 0)
        {
            for (int i = 0; i < tentacles.Length; i++)
                this.tentacles.Add(tentacles[i]);
        }

        InitializeTentacles();
    }

    private void InitializeTentacles()
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].SetupTentacle(transform);
        }
    }

    public TentacleLineRenderer GetClosestTentacleToPlayer()
    {
        TentacleLineRenderer tMin = null;

        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (TentacleLineRenderer t in tentacles)
        {
            float dist = Vector3.Distance(t.GetTentacleEndPoint(), currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}