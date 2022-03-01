using UnityEngine;

[System.Serializable]
public class Audio
{
    public string id;
    public AudioClip clip;
    public float volume;

    public Audio(string name, AudioClip clip, float volume)
    {
        this.id = name;
        this.clip = clip;
        this.volume = volume;
    }
}
