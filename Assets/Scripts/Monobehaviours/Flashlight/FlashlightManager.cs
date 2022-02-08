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

    private Light2D[] lights = new Light2D[2];

    private bool flashlightEnabled;
    private bool flickerLight;
    private float flashLightIntensity;
    private float currentLightIntensity;

    private void Start()
    {
        InitializeLights();
    }

    public void InitializeLights()
    {
        lights = GetComponentsInChildren<Light2D>();

        currentLightIntensity = defaultLightIntensity;
        flashLightIntensity = maxLightIntensity;
    }
    private void Update()
    {
        if (flashlightEnabled)
        {
            flashLightIntensity -= flashLightIntensity / batteryLifeInSeconds * Time.deltaTime;

            if (flashLightIntensity <= defaultLightIntensity)
            {
                flashLightIntensity = defaultLightIntensity;
                flashlightEnabled = false;
            }

            SetLightIntensity(flashLightIntensity);

            if(flashLightIntensity <= maxLightIntensity / 1.5f)
                StartCoroutine(FlickerLight());
        }
        else if(!flashlightEnabled)
        {
            SetLightIntensity(defaultLightIntensity);
            flickerLight = false;
        }

        if (!flickerLight)
            StopCoroutine(FlickerLight());

        currentLightIntensity = lights[0].intensity;
    }

    private void SetLightIntensity(float intensity)
    {
        for (int i = 0; i < lights.Length; i++)
            lights[i].intensity = intensity;
    }

    public void ToggleFlashlight(out bool flashLightEnabled)
    {
        if (!flashlightEnabled)
            flashlightEnabled = true;
        else
            flashlightEnabled = false;

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

        yield return new WaitForSeconds(Random.Range(0.3f, 0.8f));
        SetLightIntensity(startIntensity);

    }

    public float GetCurrentLightIntensity() => currentLightIntensity;

    public float GetCurrentMaxLightIntensity() => flashlightEnabled ? maxLightIntensity : defaultLightIntensity;
}

