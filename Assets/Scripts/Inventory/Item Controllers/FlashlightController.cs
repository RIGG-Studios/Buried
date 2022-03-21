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
    [SerializeField] private FlashlightStates state = FlashlightStates.Off;
    [SerializeField] private Light2D lightSource;

    private Battery currentBattery = null;
    private Light2D defaultLight = null;

    private float currentLightIntensity;
    private bool flashLightDisabled;
    private FlashlightSettings settings;
    private UIElement flashlightSlider;
    private UIElementGroup batteriesNeededGroup;
    private UIElement batteriesNeedText;

    private void Awake()
    {
        flashlightSlider = CanvasManager.instance.FindElementGroupByID("PlayerVitals").FindElement("flashlightslider");
        batteriesNeededGroup = CanvasManager.instance.FindElementGroupByID("BatteriesNeeded");

        if(batteriesNeededGroup != null)
        {
            batteriesNeedText = batteriesNeededGroup.FindElement("text");
        }    
    }

    private void Start()
    {
        state = FlashlightStates.Off;
    }

    public override void SetupController(Player player, Item itemInInventory)
    {
        base.SetupController(player, itemInInventory);
        defaultLight = player.defaultLight;
        settings = player.flashLightSettings;
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

    public override void ActivateItem()
    {
        UseItem();
    }

    public override void UseItem()
    {
        if (flashLightDisabled)
            return;

        if(state == FlashlightStates.Off)
        {
            if(currentBattery == null)
            {
                Item battery = null;
                player.inventory.TryFindItem(ItemProperties.ItemTypes.Battery, out battery);

                if (battery != null)
                {
                    player.inventory.UseItem(battery.item);
                }
                else
                {
                    BatteriesNeeded();
                    return;
                }
            }

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

    public override void ResetItem()
    {
  //      if (state == FlashlightStates.On)
   //         UseItem();
    }

    private void Update()
    {
        if (currentBattery == null || state == FlashlightStates.Off)
            return;

        UpdateFlashlightBatteryLevel();
    }

    private void UpdateFlashlightBatteryLevel()
    {
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

    private void BatteriesNeeded()
    {
        batteriesNeededGroup.UpdateElements(0, 0, true);
        Invoke("HideBatteriesNeeded", 0.5f);
    }

    private void HideBatteriesNeeded()
    {
        batteriesNeededGroup.UpdateElements(0, 0, false);
    }
}

