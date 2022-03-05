using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GrapplingHookSettings : ScriptableObject
{
    public float speed = 10f;
    public float maxDistance = 10f;
    public float shootSpeed = 20f;

    public LayerMask grappleLayer;
}
