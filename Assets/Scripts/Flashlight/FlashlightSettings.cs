using UnityEngine;

[CreateAssetMenu()]
public class FlashlightSettings : ScriptableObject
{
    [Header("Flashlight Properties")]
    public float maxIntensity;
    public float minIntensity;
    public float shakeDuration;
    public float shakeMagnitude;
}
