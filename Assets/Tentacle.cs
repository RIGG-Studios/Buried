using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public float targetDist;
    public float smoothSpeed;
    public LineRenderer line;
    public Vector3[] segmentPoses;
    public Transform targetDir;

    private Vector3[] segmentV;

    private void Start()
    {
        line.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    private void Update()
    {
        segmentPoses[0] = targetDir.position;

        for(int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.up * targetDist, ref segmentV[i], smoothSpeed);
        }

        line.SetPositions(segmentPoses);
    }
}
