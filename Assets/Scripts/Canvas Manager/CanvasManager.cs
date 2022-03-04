using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class handles managing of the canvas and ui elements
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    //create a list of our ui element groups, since this is the main things we modify
    private UIElementGroup[] allElements;

    private void Awake()
    {
        instance = this;

        //find the elements
        allElements = FindElements();

        //to start, play the hide animation on all elements so they aren't visible.
        foreach (UIElementGroup gr in allElements)
            gr.UpdateElements(0, 0, false);
    }

    private UIElementGroup[] FindElements()
    {
        //find all components that are a UIElementGroup underneath this gameobject
        UIElementGroup[] elements = GetComponentsInChildren<UIElementGroup>();
        //return it the array if its length is > 0.
        return elements.Length > 0 ? elements : null;
    }

    //method for finding groups based on group id
    public UIElementGroup FindElementGroupByID(string id)
    {
        //loop through all of our elements
        for(int i = 0; i < allElements.Length; i++)
        {
            //simply check if our id equals any in the list, and if so return it
            if (allElements[i].groupID == id)
                return allElements[i];
        }

        //otherwise return null
        return null;
    }

    //method for showing element groups
    public void ShowElementGroup(UIElementGroup group, bool clearOtherElements = false)
    {
        //loop through all of our elements
        foreach (UIElementGroup e in allElements)
        {
            //check which method we are using
            if (group == e)
            {
                group.UpdateElements(1, 0.25f, true);
            }
            else
            {
                if (clearOtherElements)
                    HideElementGroup(e);
            }
        }
        
    }

    public void ShowElementGroup(UIElementGroup groupObj)
    {
        UIElementGroup group = FindElementGroupByID(groupObj.groupID);

        if (group)
            ShowElementGroup(group, false);
    }

    //method for hiding specific elements
    public void HideElementGroup(UIElementGroup group)
    {
        group.UpdateElements(0, 0.25f, false);
    }
}
