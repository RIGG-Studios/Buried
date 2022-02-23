using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Segment 
{
    public int index;
    public float length;
    public float angle;
    public float frequency;
    public TentacleProperties properties;
    public Vector3 position;

    public Segment(int index, float length, float angle, float frequency, TentacleProperties properties)
    {
        this.index = index;
        this.length = length;
        this.angle = angle;
        this.frequency = frequency;
        this.properties = properties;
    }

    public Vector2 UpdatePosition(Segment previousSegment, Vector2 targetDir)
    {
        Vector2 origin = previousSegment.position;
        Vector2 dir = CalculateRotationAngle() * (targetDir - origin).normalized;

        Debug.DrawRay(origin, dir, Color.red);
        return origin + dir.normalized;
    }

    private Quaternion CalculateRotationAngle()
    {
        float rot = Mathf.PingPong(Time.time * 100f, index * properties.rotateAngle);
        return Quaternion.AngleAxis(rot, Vector3.forward);
    }
}
