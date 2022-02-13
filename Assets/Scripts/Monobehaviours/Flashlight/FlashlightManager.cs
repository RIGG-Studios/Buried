using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashlightManager : MonoBehaviour
{
    [Range(0, 500)] public float batteryLifeInSeconds;
    public float defaultLightIntensity;
    public float maxLightIntensity;
    public Slider slider;

    Light2D light;

    bool flashlightEnabled;
    bool flickerLight;

    float flashLightIntensity;
    float currentLightIntensity;

    private void Start()
    {
        InitializeLights();
    }

    public void InitializeLights()
    {
        light = GetComponentInChildren<Light2D>();

        currentLightIntensity = defaultLightIntensity;
        flashLightIntensity = maxLightIntensity;
    }

    private void Update()
    {
        if (flashlightEnabled)
        {
            flashLightIntensity -= flashLightIntensity / batteryLifeInSeconds * Time.deltaTime;
            slider.maxValue = maxLightIntensity;
            slider.minValue = defaultLightIntensity;
            slider.value = flashLightIntensity;

            if (flashLightIntensity <= defaultLightIntensity)
            {
                flashLightIntensity = defaultLightIntensity;
                slider.gameObject.SetActive(false);
                flashlightEnabled = false;
            }

            SetLightIntensity(flashLightIntensity);

            if(flashLightIntensity <= (defaultLightIntensity + maxLightIntensity) / 3)
                StartCoroutine(FlickerLight());
        }
        else 
        {
            SetLightIntensity(defaultLightIntensity);
            flickerLight = false;
        }

        if (!flickerLight)
            StopCoroutine(FlickerLight());

        currentLightIntensity = light.intensity;
    }

    private void SetLightIntensity(float intensity)
    {
        light.intensity = intensity;
    }

    public void ToggleFlashlight(out bool flashLightEnabled)
    {
        if (!flashlightEnabled)
        {
            flashlightEnabled = true;
        }
        else
        {
            flashlightEnabled = false;
        }

        slider.gameObject.SetActive(flashlightEnabled);
     
        flashLightEnabled = flashlightEnabled;
    }

    public void UpdateBatteryLife(int batteryLife)
    {
        flashLightIntensity = maxLightIntensity;
        batteryLifeInSeconds = batteryLife;
    }

    private IEnumerator FlickerLight()
    {
        float startIntensity = flashLightIntensity;

        SetLightIntensity(0);

        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        SetLightIntensity(startIntensity);

    }

    public float GetCurrentLightIntensity() => currentLightIntensity;

    public float GetCurrentMaxLightIntensity() => flashlightEnabled ? maxLightIntensity : defaultLightIntensity;
}

