using UnityEngine;

[CreateAssetMenu()]
public class TentacleProperties : ScriptableObject
{
    public enum TentacleLightResistanceLevels
    {
        High,
        Med,
        Low
    }

    public float lengthBetweenSegments;
    public float tentacleLength;
    public float aiDistance;
    public float tentacleAIMaxDistance;
    public float tentacleMoveSpeed;
    public int tentacleSegments;
    public float lightDistance;
    public TentacleLightResistanceLevels lightResistance;
}
