using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flare : MonoBehaviour, IThrowableObject
{
    public float decayOverTime;
    public float timeUntilDestroy;

    Light2D flareLight;
    bool degrade;

    float timeSinceThrown;

    void Start()
    {
        flareLight = GetComponent<Light2D>();
    }

    void Update()
    {
        if (degrade)
        {
            timeSinceThrown += Time.deltaTime;
            flareLight.intensity -= decayOverTime * Time.deltaTime;

            if (timeSinceThrown >= timeUntilDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnThrow()
    {
        degrade = true;
        timeSinceThrown = 0;
    }
}
