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

    //create a constructor for segment, where we pass in the index and tentacle properties
    public Segment(int index, TentacleProperties properties)
    {
        //assign the values
        this.index = index;
        this.properties = properties;

        position = Vector3.zero;
    }

    //method for finding the next position this segment will travel to
    public Vector2 UpdatePosition(Segment previousSegment, Vector2 targetDir, LayerMask wallLayer, float angle)
    {
        this.angle = angle;
        //the origin of this segment will be the previous segment, this it to create a chain effect
        Vector2 origin = previousSegment.position;
        //the direction will be the dir between the targetDir and origin, rotating around an axis calcuated below. Then its normalized
        Vector2 dir = CalculateRotationAngle(this.angle) * (targetDir - origin).normalized;
        //the next position will be the origin + dir
        Vector3 position = origin + dir.normalized;

        Debug.DrawRay(origin, dir, Color.red);
        //check for any walls the segment may collide with with a simple raycast in the direction we are travelling too
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dir.magnitude, wallLayer);

        //if we hit something, set the position to the collision point + the normal for a hit offset effect
        if (hit.collider != null)
            position = hit.point + hit.normal * properties.hitOffset;

        //finally return the next position after all calculations have been done
        return position;
    }

    private Quaternion CalculateRotationAngle(float angle)
    {
        //create a quaternion with the rotation amount being the index * rotate angle, this it make is to the further down the segments it travels,
        //the rotation amount will increase so it makes a wrap around effect
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
