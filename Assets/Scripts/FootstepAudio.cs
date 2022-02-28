using UnityEngine;

[System.Serializable]
public class FootstepAudio
{
    public string name;
    public AudioClip[] clips;
    public float volume;

    public FootstepAudio(string name, AudioClip[] clips, float volume)
    {
        this.name = name;
        this.clips = clips;
        this.volume = volume;
    }

    public AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
