using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicPlayer : MonoBehaviour
{
    AudioSource source;
    public AudioClip audioClip;
    bool played = false;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!played)
        {
            played = true;
            source.clip = audioClip;
            source.Play();
        }
    }
}
