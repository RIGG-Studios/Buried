using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    public LayerMask grappleLayer;

    public GameObject grappleHelper;
    public GameObject assistHelper;

    public float aimAssistSize;
    public float initialForceSpeed;

    Rigidbody2D rb;

    private bool usingGrapple;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        int layerMask = LayerMask.GetMask("GrapplePoint");

        RaycastHit2D hit = Physics2D.CircleCast(worldPos, aimAssistSize, Vector2.zero, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            assistHelper.SetActive(true);
            assistHelper.transform.position = hit.point;
        }
        else
        {
            assistHelper.SetActive(false);
        }
    }

    public bool TryStartGrapple()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        worldPos.z = 0;

        int layerMask = LayerMask.GetMask("GrapplePoint");
        RaycastHit2D hit = Physics2D.CircleCast(worldPos, aimAssistSize, transform.position - worldPos,Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            rb.velocity = Vector2.zero;
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
    //    rb.gravityScale = 0;
     //   grappleHelper.SetActive(false);
     //   usingGrapple = false;

        return true;
    }
    private void SetHelper(Vector3 position)
    {
        grappleHelper.SetActive(true);
        grappleHelper.transform.position = Vector3.zero;
        grappleHelper.transform.position = position;
    }
}
