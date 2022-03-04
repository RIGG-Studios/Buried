using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum FlashlightStates
{
    On,
    Off
}

public class FlashlightController : ItemController
{
    [SerializeField] private FlashlightStates state = FlashlightStates.On;
    [SerializeField] private FlashlightSettings settings;

    private Battery currentBattery = null;
    private Light2D lightSource = null;

    private float currentLightIntensity;
    private UIElement flashlightSlider;
    private bool flashLightDisabled;

    private void Awake()
    {
        lightSource = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        SetNewBattery(new Battery(25f, 5f));
        flashlightSlider = CanvasManager.instance.FindElementGroupByID("PlayerVitals").FindElement("flashlightslider");
    }

    public void SetNewBattery(Battery battery)
    {
        if (currentBattery == battery || battery == null)
            return;

        currentLightIntensity = settings.maxIntensity;
        currentBattery = battery;

        if (flashLightDisabled)
        {
            flashLightDisabled = false;
            UseItem();
        }
    }

    public override void UseItem()
    {
        if (flashLightDisabled)
            return;

        if(state == FlashlightStates.Off)
        {
            flashlightSlider.SetActive(true);
            lightSource.enabled = true;
            state = FlashlightStates.On;
        }
        else if(state == FlashlightStates.On)
        {
            flashlightSlider.SetActive(false);
            lightSource.enabled = false;
            state = FlashlightStates.Off;
        }
    }

    private void Update()
    {
        if (currentBattery == null || state == FlashlightStates.Off)
            return;

        currentLightIntensity -= settings.maxIntensity / currentBattery.batteryLimit * Time.deltaTime;
        lightSource.intensity = currentLightIntensity;
        currentBattery.currentBatteryLife = currentLightIntensity;

        flashlightSlider.OverrideValue(currentLightIntensity);

        if(currentLightIntensity < settings.minIntensity)
        {
            DisableItem();
        }
    }

    private void DisableItem()
    {
        state = FlashlightStates.Off;
        lightSource.intensity = 0.0f;
        currentLightIntensity = 0.0f;
        lightSource.enabled = false;
        currentBattery = null;
        flashLightDisabled = true;
    }
}
