using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flare : MonoBehaviour, IThrowableObject
{
    public float decayOverTime;
    public float timeUntilDestroy;

    Light2D flareLight;
    Rigidbody2D rb;
    bool degrade;

    float timeSinceThrown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flareLight = GetComponent<Light2D>();
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
    }

    public void OnThrow()
    {
        degrade = true;
        timeSinceThrown = 0;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        rb.AddForce(collision.contacts[0].normal * rb.velocity.magnitude);
    }
}
