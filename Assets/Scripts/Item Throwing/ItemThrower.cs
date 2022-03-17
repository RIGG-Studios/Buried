using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemThrower
{
    public static void ThrowItem(Transform throwPos, GameObject prefab, Vector2 throwDirection, float force, float decayRate, float baseIntensity)
    {
        if(prefab.GetComponent<Rigidbody2D>() != null)
        {
            GameObject instantiatedPrefab = GameObject.Instantiate(prefab);
            instantiatedPrefab.transform.position = throwPos.position;

            if (prefab.GetComponent<IThrowableObject>() == null)
            {
                Debug.LogWarning("Supplied prefab " + prefab.name + " does not contain any throw behaviour, gonna throw it anyway UmU");
            }
            else
            {
                instantiatedPrefab.GetComponent<IThrowableObject>().OnThrow();
            }

            instantiatedPrefab.GetComponent<Rigidbody2D>().AddForce(throwDirection.normalized * force, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Supplied prefab " + prefab.name + " does not contain a rigidbody UwU");
        }
    }
}
