using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Segment 
{
    //store the index of this current segment in all segments
    public int index;
    //store local tentacle properties
    public TentacleProperties properties;
    //store the position so we can move the tentacle
    public Vector3 position;
    //store the target rotation
    public float angle;
    //let us know if this segment has collided
    public bool collided;
    //length of this segment
    public float length;
    //width of this segment
    public float width;
    //the desired angle of this segment
    public float desiredAngle;
    //min length this segmnent can stretch too
    public float minLength;

    //create a constructor for segment, where we pass in the index and tentacle properties
    public Segment(int index, TentacleProperties properties, float length, float width, float desiredAngle)
    {
        //assign the values
        this.index = index;
        this.properties = properties;
        this.length = length;
        this.width = width;
        this.desiredAngle = desiredAngle;

        minLength = length / 2;
        position = Vector3.zero;
    }

    private void UpdateLength(Vector2 target)
    {
        float globalAngle = Utilites.NormalizeAngle(Mathf.Atan2(target.y - position.y, target.x - position.x));
        float currentAngle = Utilites.NormalizeAngle(globalAngle - angle);
        float angleDiff = Utilites.NormalizeAngle(currentAngle - desiredAngle);

        if(angleDiff < Mathf.PI)
        {
            float amt = Mathf.Min(0.15f, angleDiff / Mathf.PI * 0.5f);
            length = length * (1 - amt) + minLength * amt;
        }
        else
        {
            float amt = Mathf.Min(0.15f, (2 - angleDiff / Mathf.PI) * 0.5f);
            length = length * (1 - amt) + minLength * amt;
        }
    }

    //method for finding the next position this segment will travel to
    public Vector2 UpdatePosition(Segment previousSegment, Vector2 targetDir, LayerMask wallLayer)
    {
        UpdateLength(targetDir);

        angle = previousSegment.angle + Mathf.Atan2(length, width);
        //the origin of this segment will be the previous segment, this it to create a chain effect
        Vector2 origin = previousSegment.position;
        //the direction will be the dir between the targetDir and origin, rotating around an axis calcuated below. Then its normalized
        Vector2 dir = CalculateRotationAngle(angle) * (targetDir - origin).normalized;
        //the next position will be the origin + dir
        Vector2 position = origin + dir.normalized;
        //check for any walls the segment may collide with with a simple raycast in the direction we are travelling too
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dir.magnitude, wallLayer);

        collided = hit.collider != null;

        //if we hit something, set the position to the collision point + the normal for a hit offset effect
        if (collided)
        {
            position += (hit.point + hit.normal * properties.hitOffset).normalized;
        }

        //finally return the next position after all calculations have been done
        return position;
    }

    private Quaternion CalculateRotationAngle(float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
