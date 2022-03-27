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
            Debug.Log("g");
        }
        else if(state == FlashlightStates.On)
        {
            Debug.Log("g");

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
        currentLightIntensity -= settings.maxIntensity / maxIntensity * Time.deltaTime;
        lightSource.intensity = currentLightIntensity;

        flashlightSlider.OverrideValue(currentLightIntensity);

        if (currentLightIntensity < settings.minIntensity)
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
    }

    public FlashlightStates GetState()
    {
        return state;
    }
}

