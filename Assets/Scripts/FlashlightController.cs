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
    private Player player = null;

    private float currentLightIntensity;

    private void Awake()
    {
        lightSource = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        SetNewBattery(new Battery(200f, 5f));
    }

    public override void SetupController(Player player)
    {
        this.player = player;
    }

    public void SetNewBattery(Battery battery)
    {
        if (currentBattery == battery || battery == null)
            return;

        currentLightIntensity = settings.maxIntensity;
        currentBattery = battery;
    }

    public override void UseItem()
    {
        if(state == FlashlightStates.Off)
        {
            lightSource.enabled = true;
            state = FlashlightStates.On;
        }
        else if(state == FlashlightStates.On)
        {
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

        if(currentLightIntensity < settings.minIntensity)
        {
            DisableItem();
        }
    }

    private void DisableItem()
    {

    }
}
