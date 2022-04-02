using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the calculations for each segment. Since a tentacle is made of segments, each segment can stretch out or change its target rotation/direction. 
/// This class handles all of that.
/// </summary>

[System.Serializable]
public class Segment 
{
    public int index;
    public TentacleProperties properties;
    public Vector3 position;
    public float angle;
    public bool collided;
    public float length;
    public float width;
    public float desiredAngle;
    public float minLength;

    /// <summary>
    /// constructor for the segment, takes in the index in the list of all segments to calculate different rotations, length and width of the segment piece, and the desired angle.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="properties"></param>
    /// <param name="length"></param>
    /// <param name="width"></param>
    /// <param name="desiredAngle"></param>
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

    /// <summary>
    /// This method is used to update the length of the segment. This to create a "stretch" effect for the tentacle so it behaves realistically and organically.
    /// Updates the length based on the angle difference between the target and the segment.
    /// </summary>
    /// <param name="target"></param>
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


    /// <summary>
    /// This method is used to find the next position of the segment, needing the previous segment to create a chain effect.
    /// </summary>
    /// <param name="previousSegment"></param>
    /// <param name="targetDir"></param>
    /// <param name="wallLayer"></param>
    /// <returns></returns>
    public Vector2 UpdatePosition(Segment previousSegment, Vector2 targetDir, LayerMask wallLayer)
    {
        UpdateLength(targetDir);

        angle = previousSegment.angle + Mathf.Atan2(length, width);
        //the origin of this segment will be the previous segment, this it to create a chain effect
        Vector2 origin = previousSegment.position;
        //the direction will be the dir between the targetDir and origin, rotating around an axis calcuated below. Then its normalized
        Vector2 dir = CalculateRotationAngle(angle) * (targetDir - origin).normalized;
        //the next position will be the origin + dir
        Vector2 position = (origin + dir.normalized) * properties.lengthBetweenSegments;
        //check for any walls the segment may collide with with a simple raycast in the direction we are travelling too
        RaycastHit2D hit = Physics2D.Raycast(origin, dir.normalized, dir.magnitude, wallLayer);

        collided = hit.collider != null;

        Debug.DrawRay(origin, dir, Color.red);
        //if we hit something, set the position to the collision point + the normal for a hit offset effect
        if (collided)
        {
            Vector2 next = (CalculateRotationAngle(angle) * hit.point).normalized;
            position += next;
        }

       
        //finally return the next position after all calculations have been done
        return position;
    }

    /// <summary>
    /// simple method for rotating around an axis. In this class the forward axis.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Quaternion CalculateRotationAngle(float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
