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
    [Header("Flashlight Positions")]
    [SerializeField] private Vector3 moveRightPosition = Vector3.zero;
    [SerializeField] private Vector3 moveLeftPosition = Vector3.zero;
    [SerializeField] private Vector3 moveUpPosition = Vector3.zero;
    [SerializeField] private Vector3 moveDownPosition = Vector3.zero;

    [SerializeField] private FlashlightStates state = FlashlightStates.Off;
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
        state = FlashlightStates.Off;
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

    public override void ResetItem()
    {
        if (state == FlashlightStates.On)
            UseItem();
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, GetFlashlightPosition(), Time.deltaTime * 10f);

        UpdateFlashlight();
    }

    private void UpdateFlashlight()
    {
        if (currentBattery == null || state == FlashlightStates.Off)
            return;

        currentLightIntensity -= settings.maxIntensity / currentBattery.batteryLimit * Time.deltaTime;
        lightSource.intensity = currentLightIntensity;
        currentBattery.currentBatteryLife = currentLightIntensity;

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
        currentBattery = null;
        flashLightDisabled = true;
    }

    public FlashlightStates GetState()
    {
        return state;
    }

    private Vector3 GetFlashlightPosition()
    {
        MovementState state = player.stateManager.currentState.name == "PlayerMovement" ? (MovementState)player.stateManager.currentState : null;

        if (state == null)
            return startingPosition;

        if (state != null)
        {
            int dir = state.GetDirection();

            switch (dir)
            {
                case 0:
                    return moveUpPosition;

                case 1:
                    return moveLeftPosition;

                case 2:
                    return moveDownPosition;

                case 3:
                    return moveRightPosition;
            }
        }

        return startingPosition;
    }
}

