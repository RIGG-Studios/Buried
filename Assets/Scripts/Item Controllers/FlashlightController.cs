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
        flashlightSlider = player.playerCanvas.FindElementGroupByID("PlayerFlashlightGroup").FindElement("flashlightslider");
        currentLightIntensity = settings.maxIntensity;
    }


    public void ToggleFlashLight()
    {
        if(state == FlashlightStates.Off)
        {
            Debug.Log(gameObject.name);
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
                ToggleFlashlightSlider(true);
            else
                ToggleFlashlightSlider(false);
        }
        else if(state == FlashlightStates.On)
        {
            if (!chargeBattery)
                currentLightIntensity -= settings.maxIntensity / maxIntensity * Time.deltaTime;
        }

        lightSource.intensity = currentLightIntensity;

        flashlightSlider.OverrideValue(currentLightIntensity / settings.maxIntensity);

        if (currentLightIntensity < settings.minIntensity && !chargeBattery)
        {
            DisableItem();
        }
    }

    public bool GetIsFullyCharged() => currentLightIntensity >= settings.maxIntensity;

    public void ToggleChargeBattery(bool state) => chargeBattery = state;

    void ToggleFlashlightSlider(bool on) => flashlightSlider.SetActive(on);

    private void DisableItem()
    {
        state = FlashlightStates.Off;
        lightSource.intensity = 0.0f;
        currentLightIntensity = 0.0f;
        lightSource.enabled = false;
        ToggleFlashlightSlider(false);
    }

    public FlashlightStates GetState()
    {
        return state;
    }
}

