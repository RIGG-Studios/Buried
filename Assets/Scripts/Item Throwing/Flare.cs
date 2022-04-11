using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flare : MonoBehaviour, IThrowableObject
{
    [SerializeField] private float decayOverTime;
    [SerializeField] private float timeUntilDestroy;
    [SerializeField] private float fakeHeight;
    [SerializeField] private Transform flareIcon;

    Light2D flareLight;
    Rigidbody2D rb;
    BoxCollider2D collider;
    bool degrade;
    bool hit;
    int hitCount;

    float timeSinceThrown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flareLight = GetComponent<Light2D>();
        collider = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        if (degrade)
        {
            timeSinceThrown += Time.deltaTime;
            flareLight.intensity -= decayOverTime * Time.deltaTime;

            if (timeSinceThrown >= timeUntilDestroy)
            {
                Destroy(gameObject);
            }
        }

        float velocity = rb.velocity.magnitude;
        float target = velocity > 0.6f ? 180f : 0.0f;

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, Random.Range(-target, target)), Time.deltaTime * velocity);
    }

    public void OnThrow()
    {
        degrade = true;
        timeSinceThrown = 0;
    }
}
