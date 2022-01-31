using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    public float initialForceSpeed;

    public LayerMask grappleLayer;
    public GameObject grappleHelper;

    Rigidbody2D rb;

    private bool usingGrapple;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public bool TryStartGrapple()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        worldPos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, grappleLayer);


        if (hit.collider != null)
        {
            rb.velocity = Vector2.zero;
            float dot = Vector3.Dot(transform.position, hit.point);
            rb.gravityScale = 1;

            Vector2 dir = (transform.position - worldPos).normalized;
            rb.AddForce(new Vector2(-dir.x, 0) * initialForceSpeed);

            SetHelper(hit.point);
            usingGrapple = true;
        }

        return usingGrapple;
    }

    public bool TryEndGrapple()
    {
        rb.gravityScale = 0;
        grappleHelper.SetActive(false);
        usingGrapple = false;

        return true;
    }


    private void SetHelper(Vector3 position)
    {
        grappleHelper.SetActive(true);
        grappleHelper.transform.position = Vector3.zero;
        grappleHelper.transform.position = position;
    }
}
