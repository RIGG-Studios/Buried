using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleCanvas : MonoBehaviour
{
    public static GrappleCanvas instance;

    public bool disabled { get; private set; }

    [SerializeField] private GameObject text;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void Disable()
    {
        text.SetActive(false);
        disabled = true;
    }
}
