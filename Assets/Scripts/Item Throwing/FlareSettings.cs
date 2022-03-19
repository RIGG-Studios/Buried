using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FlareSettings : ScriptableObject
{
    public float throwForce;
    public float bounceForce;
    public float decayRate;
    public float baseIntensity;
    public GameObject flarePrefab;
}
