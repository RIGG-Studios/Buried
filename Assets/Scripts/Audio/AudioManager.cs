using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Audio> audioInScene = new List<Audio>();

    private AudioSource source = null;
    private Audio currentAudio;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.OnPlayAudio += PlayAudio;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayAudio -= PlayAudio;
    }

    public void PlayAudio(string audioName)
    {
        if (!source.isPlaying)
        {
            if (currentAudio != null && currentAudio.id == audioName)
            {
                PlayAudio(currentAudio);
                return;
            }

            Audio aud = FindAudio(audioName);
            currentAudio = aud != null ? aud : null;

            PlayAudio(currentAudio);
        }

    }
    public Audio FindAudio(string name)
    {
        for(int i = 0; i < audioInScene.Count; i++)
        {
            if (name == audioInScene[i].id)
                return audioInScene[i];
        }

        return null;
    }

    public void PlayAudio(Audio audio)
    {
        source.volume = audio.volume;
        source.PlayOneShot(audio.clip);
    }

    public void CreateNewAudio(string name, AudioClip clip, float volume, out Audio audio)
    {
        Audio aud = new Audio(name, clip, volume);

        if(aud != null && !audioInScene.Contains(aud))
        {
            audioInScene.Add(aud);
        }

        audio = aud;
    }
}
