using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

//since every UI element must have some variables/methods
//in common, this class will handle all that.
public class UIElement : MonoBehaviour
{
    //every ui element will have an id we can use to find it through script, and also graphics.
    public string id;
    public MaskableGraphic graphics;


    //method for finding the transform
    public Transform GetTransform() => transform;

    //method for setting ui elements active/disabled
    public void SetActive(bool state) => gameObject.SetActive(state);

    //method for overriding data of ui elements, eg texts data will be a string, while an image data will be a sprite.
    public virtual void OverrideValue(object message) { }
    public virtual void OverrideColor(Color color) => graphics.color = color;

    public virtual void PlayAnimation(string name)
    {
        Animator animator = GetComponent<Animator>();

        if (animator != null)
            animator.SetTrigger(name);
    }

    //method for assigning the id and graphics easily through custom editors
    public void Setup()
    {
        id = gameObject.name.ToLower();
        graphics = GetComponent<MaskableGraphic>();
    }

    public void FadeElement(float alpha, float duration) => StartCoroutine(IELerpAlpha(alpha, duration));

    private IEnumerator IELerpAlpha(float targetAlpha, float duration)
    {
        //create a temp time variable
        float t = 0;

        //while our time is less than 1, lerp towards a value
        while (t < 1)
        {
            t += Time.deltaTime;// increase time

            graphics.CrossFadeAlpha(targetAlpha, duration, false);

            yield return null;
        }
    }
}


