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

    Light2D[] lights = new Light2D[2];

    bool flashlightEnabled;
    bool flickerLight;

    float flashLightIntensity;
    float currentLightIntensity;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        lights = GetComponentsInChildren<Light2D>();

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

   //     lights[1].enabled = player.isHiding;
        currentLightIntensity = lights[0].intensity;
    }

    private void SetLightIntensity(float intensity)
    {
        foreach(Light2D light in lights)
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

