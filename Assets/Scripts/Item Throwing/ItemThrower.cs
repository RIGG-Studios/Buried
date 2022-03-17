using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public static class ItemThrower
{
    public static void ThrowItem(Transform throwPos, GameObject prefab, Vector2 throwDirection, float force, float decayRate, float baseIntensity)
    {
        if(prefab.GetComponent<Rigidbody2D>() != null)
        {
            GameObject instantiatedPrefab = GameObject.Instantiate(prefab);
            instantiatedPrefab.transform.position = throwPos.position;

            bool doLightStuff;

            if (prefab.GetComponent<Light2D>() != null)
            {
                doLightStuff = true;
            }
            else
            {
                doLightStuff = false;
                Debug.LogWarning("Supplied prefab " + prefab.name + " does not contain a 2d light UwU, gonna throw it anyway");
            }

            instantiatedPrefab.GetComponent<Rigidbody2D>().AddForce(throwDirection.normalized * force, ForceMode2D.Impulse);


        }
        else
        {
            Debug.LogError("Supplied prefab " + prefab.name + " does not contain a rigidbody UwU");
        }
    }
}
