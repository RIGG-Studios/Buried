using UnityEngine;

[CreateAssetMenu()]
public class FlashlightSettings : ScriptableObject
{
    [Header("Flashlight Properties")]
    [Range(0, 5)] public float maxIntensity;
    [Range(0, 5)] public float minIntensity;
}
