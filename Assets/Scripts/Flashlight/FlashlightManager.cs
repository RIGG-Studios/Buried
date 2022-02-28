using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class FlashlightManager : MonoBehaviour
{
    [Header("Flashlight Properties")]
    [SerializeField, Range(0, 500)] private float batteryLifeInSeconds;
    [SerializeField, Range(0, 5)] float defaultLightIntensity;
    [SerializeField, Range(0, 5)] float maxLightIntensity;

    [Header("UI")]
    [SerializeField] private Slider slider;

    private Light2D[] lights = new Light2D[2];
    private bool flashlightEnabled = false;
    private bool flickerLight = false;
    private float flashLightIntensity = 0.0f;
    private float currentLightIntensity = 0.0f;
    private Player player;

    private void Awake()
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

        currentLightIntensity = lights[0].intensity;
    }

    private void SetLightIntensity(float intensity)
    {
        foreach(Light2D light in lights)
            light.intensity = intensity;
    }

    public void ToggleFlashlight()
    {
        Debug.Log("g");
        if (!flashlightEnabled)
        {
            slider.maxValue = maxLightIntensity;
            slider.minValue = defaultLightIntensity;
            flashlightEnabled = true;
        }
        else
        {
            flashlightEnabled = false;
        }

        slider.gameObject.SetActive(flashlightEnabled);

        GameEvents.OnToggleFlashlight(flashlightEnabled);
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

    public float GetCurrentLightIntensity()
    {
       return currentLightIntensity;
    }
    public float GetCurrentMaxLightIntensity()
    {
        return flashlightEnabled ? maxLightIntensity : defaultLightIntensity;
    }
}

