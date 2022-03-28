using UnityEngine;
using UnityEngine.InputSystem; 

public class MouseLook : MonoBehaviour
{
    private void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 dir = (worldPos - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
    }
}
