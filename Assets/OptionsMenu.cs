using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown = null;
    [SerializeField] private AudioMixer mixer = null;

    private Resolution[] resolutions;

    private void Awake()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int index = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                index = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = index;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }

    public void ToggleFullScreen(bool toggle)
    {
        Screen.fullScreen = toggle;
    }

    public void SetResolution(int i)
    {
        Resolution res = resolutions[i];

        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}


