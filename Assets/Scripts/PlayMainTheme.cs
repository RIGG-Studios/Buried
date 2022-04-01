using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMainTheme : MonoBehaviour
{
    public AudioClip mainTheme;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = mainTheme;

        source.Play();
    }
}
