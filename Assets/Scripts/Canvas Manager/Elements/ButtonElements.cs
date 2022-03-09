using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonElements : UIElement
{
    public Button button
    {
        get
        {
            return GetComponent<Button>();
        }
    }

    public override void OverrideValue(object message)
    {
        base.OverrideValue(message);
    }
}
