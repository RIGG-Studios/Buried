using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class MouseLook : MonoBehaviour
{
    public float clamp;

    private void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 dir = (worldPos - transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, dir);
        Vector3 eulerAngles = rotation.eulerAngles;
        transform.eulerAngles = eulerAngles;
    }
}
