using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery 
{
    public float batteryLimit;
    public float currentBatteryLife;

    public Battery(float batteryLimit, float currentBatteryLife)
    {
        this.batteryLimit = batteryLimit;
        this.currentBatteryLife = currentBatteryLife;
    }
}
