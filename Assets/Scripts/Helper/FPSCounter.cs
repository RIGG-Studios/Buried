using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    Text text;
    float currentFPS;
    void Start()
    {
        text = GetComponent<Text>();

        InvokeRepeating("GetFPS", 0.2f, 0.2f);
    }

    public void GetFPS()
    {
        currentFPS = (int)(1f / Time.unscaledDeltaTime);
        text.text = "FPS: " + currentFPS.ToString();
    }

}
