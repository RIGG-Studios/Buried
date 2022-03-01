using UnityEngine;

[CreateAssetMenu()]
public class MovementSettings : ScriptableObject
{
    public float movementSpeed;
    public float movementParanoidMultiplier;

    public float cameraOffset;

    public float stepRate;
    public float stepRateParanoidMultiplier;
}
