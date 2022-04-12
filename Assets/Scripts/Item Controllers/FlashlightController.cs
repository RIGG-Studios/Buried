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

    private float currentLightIntensity;
    private UIElement flashlightSlider;
    private UIElementGroup flashlightRunningLow;
    private UIElementGroup flashlightRecharging;
    private Player player;
    private bool chargeBattery;


    private void Start()
    {
        player = FindObjectOfType<Player>();
        state = FlashlightStates.Off;
        player.playerInput.Player.Flashlight.performed += ctx => ToggleFlashLight();
        flashlightSlider = player.playerCanvas.FindElementGroupByID("PlayerFlashlightGroup").FindElement("flashlightslider");
        flashlightRunningLow = player.playerCanvas.FindElementGroupByID("FlashlightRunningLowGroup");
        flashlightRecharging = player.playerCanvas.FindElementGroupByID("FlashlightRechargingGroup");
        currentLightIntensity = settings.maxIntensity;
    }


    public void ToggleFlashLight()
    {
        if(state == FlashlightStates.Off)
        {
            ToggleFlashlightSlider(true);
            lightSource.enabled = true;
            state = FlashlightStates.On;
        }
        else if(state == FlashlightStates.On)
        {
            ToggleFlashlightSlider(false);
            lightSource.enabled = false;
            state = FlashlightStates.Off;
        }
    }


    private void Update()
    {
        if (player.stateManager.GetStateInEnum(player.stateManager.currentState) == PlayerStates.Hiding && state == FlashlightStates.On)
            ToggleFlashLight();
            
        if (chargeBattery && currentLightIntensity <= settings.maxIntensity)
        {
            currentLightIntensity += settings.maxIntensity / maxIntensity * Time.deltaTime;
        }

        UpdateFlashlightBatteryLevel();
    }


    private void UpdateFlashlightBatteryLevel()
    {
        if (state == FlashlightStates.Off)
        {
            if (chargeBattery)
            {
                ToggleFlashlightSlider(true);
            }
            else
            {
                ToggleFlashlightSlider(false);
            }
        }
        else if(state == FlashlightStates.On)
        {
            if (!chargeBattery)
                currentLightIntensity -= settings.maxIntensity / maxIntensity * Time.deltaTime;
        }

        lightSource.intensity = currentLightIntensity;

        flashlightSlider.OverrideValue(1 - Utilites.NormalizeToRange(currentLightIntensity, settings.minIntensity, settings.maxIntensity));

        if (currentLightIntensity < settings.minIntensity && !chargeBattery)
        {
            DisableItem();
        }

        if(currentLightIntensity <= settings.maxIntensity / 2.5f && !chargeBattery && state == FlashlightStates.On)
        {
            flashlightRunningLow.UpdateElements(0, 0, true);
        }
        else
        {
            flashlightRunningLow.UpdateElements(0, 0, false);
        }
    }

    public bool GetIsFullyCharged() => currentLightIntensity >= settings.maxIntensity;

    public void ToggleChargeBattery(bool state) 
    {
        flashlightRecharging.UpdateElements(0, 0, state);
        chargeBattery = state;
    }

    void ToggleFlashlightSlider(bool on) => flashlightSlider.SetActive(on);

    private void DisableItem()
    {
        state = FlashlightStates.Off;
        lightSource.intensity = settings.minIntensity ;
        currentLightIntensity = 0.0f;
        lightSource.enabled = false;
        flashlightRunningLow.UpdateElements(0, 0, false);
        ToggleFlashlightSlider(false);
    }

    public FlashlightStates GetState()
    {
        return state;
    }
}

