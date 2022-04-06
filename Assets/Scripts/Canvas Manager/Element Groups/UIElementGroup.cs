using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum UpdateTypes
{
    Animation,
    Fade,
    Static
}

//this script handles grouping of different ui elements
[RequireComponent(typeof(Animator))]
public class UIElementGroup : MonoBehaviour
{
    //id for the group
    public string groupID;

    //list of elements this group contains
    public UIElement[] elementsInGroup;

    public UpdateTypes hideType;
    public bool active { get; private set; }

    //create a property of our animator
    private Animator animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }

    //method for finding elements
    public UIElement FindElement(string id)
    {
        for(int i = 0; i < elementsInGroup.Length; i++)
        {
            if (id == elementsInGroup[i].id)
            {
                return elementsInGroup[i];
            }
        }
        return null;
    }

    public void UpdateElements(float targetAlpha, float duration, bool show)
    {
        switch (hideType)
        {
            case UpdateTypes.Static:
                UpdateStatic(show);
                break;

            case UpdateTypes.Animation:
                PlayAnimation(show);
                break;

            case UpdateTypes.Fade:
                Fade(targetAlpha, duration);
                break;
        }
    }

    private void UpdateStatic(bool show)
    {
        for (int i = 0; i < elementsInGroup.Length; i++)
        {
            elementsInGroup[i].gameObject.SetActive(show);
        }
    }

    //method for playing animations
    private void PlayAnimation(bool show)
    {
        if (show)
        {
            animator.SetTrigger("show");
            active = true;
        }
        else
        {
            animator.SetTrigger("hide");
            active = false;
        }
    }

    private void Fade(float alpha, float dur)
    {
        if (alpha == 1)
        {
            for (int i = 0; i < elementsInGroup.Length; i++) //for every element in the group
                elementsInGroup[i].gameObject.SetActive(true);

            active = true;
        }

        StartCoroutine(IELerpAlpha(alpha, dur));
    }

    private IEnumerator IELerpAlpha(float targetAlpha, float duration)
    {
        //create a temp time variable
        float t = 0;

        //while our time is less than 1, lerp towards a value
        while (t < 1)
        {
            t += Time.deltaTime;// increase time

            for (int i = 0; i < elementsInGroup.Length; i++) //for every element in the group
            {
                //cross fade the graphics to the desired alpha, based on the duration
                elementsInGroup[i].graphics.CrossFadeAlpha(targetAlpha, duration, true);
            }

            yield return null;
        }

       if (targetAlpha == 1)
        {
            active = true;
        }
    }

    //method for our editor script to find all ui elements underneath this obj, and assign the
    //id to the name of the gameobject
    public void FindUIElements()
    {
        UIElement[] elements = GetComponentsInChildren<UIElement>();
        groupID = gameObject.name;

        if (elements.Length > 0)
            elementsInGroup = elements;
        else
        {
            Text[] texts = GetComponentsInChildren<Text>();

            if (texts.Length > 0)
            {
                foreach (Text txt in texts)
                {
                    txt.gameObject.AddComponent<TextElement>().Setup();
                }
            }

            Image[] images = GetComponentsInChildren<Image>();

            if (images.Length > 0)
            {
                foreach (Image img in images)
                {
                    img.gameObject.AddComponent<ImageElement>().Setup();
                }
            }

            Slider[] sliders = GetComponentsInChildren<Slider>();

            if(sliders.Length > 0)
            {
                foreach(Slider sld in sliders)
                {
                    sld.gameObject.AddComponent<SliderElement>().Setup();
                }
            }

            Button[] buttons = GetComponentsInChildren<Button>();

            if(buttons.Length > 0)
            {
                foreach(Button btn in buttons)
                {
                    btn.gameObject.AddComponent<ButtonElements>().Setup();
                }
            }


            UIElement[] elem = GetComponentsInChildren<UIElement>();

            if (elem.Length > 0)
                elementsInGroup = elem;
        }
    } 
}
