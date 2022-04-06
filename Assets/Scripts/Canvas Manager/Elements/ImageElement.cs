using UnityEngine.UI;
using UnityEngine;
using System;
//this class will be attached to any image component, so we can modify it through script

public class ImageElement : UIElement
{
    private Image Image
    {
        get
        {
            return GetComponent<Image>();
        }
    }

    //override method for the base UIElement, essentially casting the object from an object to a sprite
    //and assiogning the image sprite to the message
    public override void OverrideValue(object message)
    {
        Type type = message.GetType();

        if (type.Equals(typeof(Sprite)))
        {
            Sprite sprite = (Sprite)message;

            if (sprite != null)
                Image.sprite = sprite;
        }
        else
        {
            float amt = (float)message;

            Image.fillAmount = amt;
        }
    }

    public void SetNatizeSize() => Image.SetNativeSize();

}
