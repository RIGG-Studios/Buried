using UnityEngine;

[CreateAssetMenu()]
public class TentacleProperties : ScriptableObject
{
    [Header("Base Properties")]
    public float tentacleMaxLength;
    public float tentacleMoveSpeed;
    public float lightDistance;
    public float audioFrequency;

    [Header("Segments Properties")]
    public int tentacleSegments;
    public float lengthBetweenSegments;
    public float hitOffset;
    public float detectionRange;

    [Header("Audio")]
    public AudioClip[] tentacleAudio;

    public AudioClip GetRandomAudio() => tentacleAudio[Random.Range(0, tentacleAudio.Length)];
}
