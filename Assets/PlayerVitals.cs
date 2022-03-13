using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{
    private Player player = null;
    private PostProcessingManager postProcessingManager = null;

    public float currentHealth { get; private set; }
    public float currentOxygen { get; private set; }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        postProcessingManager = FindObjectOfType<PostProcessingManager>();
    }

    private void Start()
    {
        currentHealth = player.vitalSettings.maxHealth;
        currentOxygen = player.vitalSettings.maxOxygen;
    }
}
