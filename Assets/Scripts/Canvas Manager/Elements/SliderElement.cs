using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderElement : UIElement
{
    private Slider slider
    {
        get
        {
            return GetComponent<Slider>();
        }
    }

    public override void OverrideValue(object message)
    {
        float value = (float)message;
        slider.value = value;
    }
}
