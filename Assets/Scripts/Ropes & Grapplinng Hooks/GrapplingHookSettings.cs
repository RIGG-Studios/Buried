using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GrapplingHookSettings : ScriptableObject
{
    public float maxDistance = 10f;
    public float shootSpeed = 20f;
    public int percision = 20;
    public float waveSize;
    public float launchSpeedMultiplier;
    public float straightenLineSpeed;

    public AnimationCurve ropeAnimationCurve;
    public AnimationCurve ropeLaunchSpeedCurve;
    public LayerMask grappleLayer;
}
