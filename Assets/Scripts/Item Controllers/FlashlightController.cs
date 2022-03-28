using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum FlashlightStates
{
    On,
    Off
}

public class FlashlightController : MonoBehaviour
{

    [SerializeField] private FlashlightStates state = FlashlightStates.Off;
    [SerializeField] private FlashlightSettings settings = null;
    [SerializeField] private float maxIntensity = 0.0f;
    [SerializeField] private Light2D lightSource = null;
    [SerializeField] private Light2D defaultLight = null;

    private float currentLightIntensity;
    private UIElement flashlightSlider;
    private Player player;
    private bool chargeBattery;


    private void Start()
    {
        player = FindObjectOfType<Player>();
        state = FlashlightStates.Off;
        player.playerInput.Player.Flashlight.performed += ctx => ToggleFlashLight();
        flashlightSlider = CanvasManager.instance.FindElementGroupByID("PlayerVitals").FindElement("flashlightslider");
        currentLightIntensity = settings.maxIntensity;
    }


    public void ToggleFlashLight()
    {
        if(state == FlashlightStates.Off)
        {
            flashlightSlider.SetActive(true);
            lightSource.enabled = true;
            defaultLight.enabled = false;
            state = FlashlightStates.On;
        }
        else if(state == FlashlightStates.On)
        {
            flashlightSlider.SetActive(false);
            lightSource.enabled = false;
            defaultLight.enabled = true;
            state = FlashlightStates.Off;
        }
    }


    private void Update()
    {
        if (state == FlashlightStates.Off)
            return;

        UpdateFlashlightBatteryLevel();
    }


    private void UpdateFlashlightBatteryLevel()
    {
        if(chargeBattery)
            currentLightIntensity += settings.maxIntensity / maxIntensity * Time.deltaTime;
        else
            currentLightIntensity -= settings.maxIntensity / maxIntensity * Time.deltaTime;

        lightSource.intensity = currentLightIntensity;

        flashlightSlider.OverrideValue(currentLightIntensity);

        if (currentLightIntensity < settings.minIntensity)
        {
            DisableItem();
        }
    }

    public void ToggleChargeBattery(bool state) => chargeBattery = state;  

    private void DisableItem()
    {
        state = FlashlightStates.Off;
        lightSource.intensity = 0.0f;
        currentLightIntensity = 0.0f;
        lightSource.enabled = false;
        flashlightSlider.SetActive(false);
    }

    public FlashlightStates GetState()
    {
        return state;
    }
}

