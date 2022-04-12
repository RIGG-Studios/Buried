using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    private Resolution resolution;
    public float volume { get; private set; }
    public bool fullscreen { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        volume = PlayerPrefs.GetFloat("volume");
        fullscreen = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;
    }

    public void SetRes(Resolution resolution)
    {
        this.resolution = resolution;
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool fullscreen)
    {
        this.fullscreen = fullscreen;
        PlayerPrefs.SetInt("fullscreen", fullscreen ? 1 : 0);
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetInt("fullscreen", fullscreen ? 1 : 0);

    }
}
