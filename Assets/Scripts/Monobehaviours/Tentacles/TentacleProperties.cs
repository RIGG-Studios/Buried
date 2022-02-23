using UnityEngine;

[CreateAssetMenu()]
public class TentacleProperties : ScriptableObject
{
    [Header("Base Properties")]
    public float tentacleMaxLength;
    public float tentacleMoveSpeed;
    public float lightDistance;

    [Header("Segments Properties")]
    public int tentacleSegments;
    public float lengthBetweenSegments;
    public float hitOffset;
    public float rotateAngle;
}
