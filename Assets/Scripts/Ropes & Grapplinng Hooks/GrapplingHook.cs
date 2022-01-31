using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrapplingHook : MonoBehaviour
{
    private List<Vector2> ropePositions = new List<Vector2>();

    public Rigidbody2D rb;
    public float speed;
    public LayerMask layer;
    public GameObject joint;

    bool ingrapple;
    Rope rope;
    public LineRenderer lineRender;

    private void Update()
    {
        if (Input.GetMouseButton(0) && !ingrapple)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            if (hit.collider != null)
            {
                rb.gravityScale = 1;
                GetComponent<PlayerMovement>().enabled = false;
                Vector2 dir = (transform.position - point).normalized;
                rb.AddForce(new Vector2(-dir.x, 0) * speed);
                rb.velocity = Vector2.zero;
                SpawnDummy(hit.transform);
                ingrapple = true;
            }
        }

        if(ingrapple && Input.GetMouseButtonUp(0))
        {
            rb.gravityScale = 0;
            joint.SetActive(false);
            GetComponent<PlayerMovement>().enabled = true;

            ingrapple = false;
        }
    }


    private void SpawnDummy(Transform position)
    {
        joint.SetActive(true);
        rope = joint.GetComponent<Rope>();
     //   rope.SetupSwing(transform, position);
        joint.transform.position = position.position;
    }
}
