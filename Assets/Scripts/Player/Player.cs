using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private float maxHealth;

    [SerializeField, Range(0, 10)]
    private float maxOxygen;

    public float health { get; private set; }
    public float oxygen { get; private set; }

    private void Start()
    {
        health = maxHealth;
        oxygen = maxOxygen;
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void DepleteOxygen(float amount)
    {
        oxygen -= amount;

        if(oxygen <= 0)
        {
            //start a damage loop as we are now out of breath
        }
    }

    private void Die()
    {
        Debug.Log("Now Dead");
    }
}

