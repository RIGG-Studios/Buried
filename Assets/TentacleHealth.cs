using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleHealth : MonoBehaviour
{
    [Header("Health Values")]

    [SerializeField, Range(0, 100)] public float maxHealth;
    [SerializeField, Range(0, 100)] public float startingHealth;

    [Header("Slider")]

    private TentacleController controller;

    public float currentHealth { get; private set; }

    private void Awake()
    {
        controller = GetComponent<TentacleController>();
    }
}
