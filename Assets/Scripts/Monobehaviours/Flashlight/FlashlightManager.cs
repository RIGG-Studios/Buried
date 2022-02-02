using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashlightManager : MonoBehaviour
{
    public float batteryLifeInSeconds;
    public Light2D lightSource;
    public Slider flashlightSlider;

    float currentBattery;
    bool flashlightEnabled;

    private void Start()
    {
        currentBattery = lightSource.intensity;
        flashlightSlider.maxValue = lightSource.intensity;
        flashlightSlider.value = flashlightSlider.maxValue;

        ToggleFlashlight();
    }

    private void Update()
    {
        if (flashlightEnabled)
        {
            lightSource.intensity -= currentBattery / batteryLifeInSeconds * Time.deltaTime;

            flashlightSlider.value = lightSource.intensity;

            if (lightSource.intensity <= 0)
                ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        if(flashlightEnabled)
        {
            flashlightSlider.gameObject.SetActive(false);
            lightSource.enabled = false;
            flashlightEnabled = false;
        }
        else if(!flashlightEnabled)
        {
            flashlightSlider.gameObject.SetActive(true);
            lightSource.enabled = true;
            flashlightEnabled = true;
        }
    }
}

