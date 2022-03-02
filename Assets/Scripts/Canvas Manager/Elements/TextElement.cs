using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

//this class will be attached to any text component, so we can modify it through script
public class TextElement : UIElement
{ 
    //we need reference to the text component
    private Text Text
    {
        get
        {
            return GetComponent<Text>();
        }
    }

    //override method for the base UIElement, essentially casting the object from an object to a string
    //and assiogning the text value to the message.
    public override void OverrideValue(object message)
    {
        string msg = (string)message;

        if (msg != null)
            Text.text = msg;
    }
}
