using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class PostProcessingManager : MonoBehaviour
{
    private Volume volume = null;

    private Vignette vignette = null;
    private ChromaticAberration chromaticAberration = null;
    private LensDistortion lensDistortion = null;

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }

    public void OnOxygenDepleted(float amount, float t)
    {
        float lensX = Mathf.PingPong(t, amount);
    }

    public void OnHealthDepleted(float amount)
    {

    }
}
