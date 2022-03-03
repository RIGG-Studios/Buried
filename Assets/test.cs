using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Image bar;
    public float health;

    private void Update()
    {
        bar.fillAmount = Mathf.Lerp(bar.fillAmount, health, Time.deltaTime * 5f);
    }
}
