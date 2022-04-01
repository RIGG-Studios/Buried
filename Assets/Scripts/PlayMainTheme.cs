using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMainTheme : MonoBehaviour
{
    public AudioClip mainTheme;
    AudioSource source;

    void Start()
    {
        Debug.Log(transform.parent);
        Debug.Log(gameObject);
        source = GetComponent<AudioSource>();
        source.clip = mainTheme;

        source.Play();
    }
}
