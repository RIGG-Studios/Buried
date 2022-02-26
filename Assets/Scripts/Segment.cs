using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Segment 
{
    public int index;
    public float length;
    public float angle;
    public TentacleProperties properties;
    public Vector3 position;
    public float rotationAngle;

    public Segment(int index, float length, float angle, TentacleProperties properties)
    {
        this.index = index;
        this.length = length;
        this.angle = angle;
        this.properties = properties;

        position = Vector3.zero;
        rotationAngle = properties.rotateAngle;
    }

    public Vector2 UpdatePosition(Segment previousSegment, Vector2 targetDir, LayerMask wallLayer, float targetRotation)
    {
        rotationAngle = targetRotation;

        Vector2 origin = previousSegment.position;
        Vector2 dir = CalculateRotationAngle() * (targetDir - origin).normalized;
        Vector3 position = origin + dir;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dir.magnitude, wallLayer);

        if (hit.collider != null)
            position = hit.point + hit.normal * properties.hitOffset;

        return position;
    }

    private Quaternion CalculateRotationAngle()
    {
        return Quaternion.AngleAxis(index * properties.rotateAngle, Vector3.forward);
    }
}
