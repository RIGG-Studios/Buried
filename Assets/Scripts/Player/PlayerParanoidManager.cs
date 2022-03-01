using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParanoidManager : MonoBehaviour
{
    [Range(0.1f, 1)] public float paranoidAmount;

    [SerializeField, Range(0, 100f)] private float enemyDistanceThreshold;
    [SerializeField, Range(0, 100f)] private float enemyDistanceMultiplier;

    [SerializeField] private float heartBeatMultiplier = 1;
    [SerializeField] private float cameraShakeMultiplier = 1;

    private float currentHeartBeat = 0.0f;
    private float currentShakeMagnitude = 0.0f;
    private float heartBeatTimer = 0.0f;
    private float distance = 0.0f;

    private bool calculateDistance = false;
    private TentacleController controller = null;
    private PlayerCamera playerCamera = null;

    private void Awake()
    {
        distance = 100f;
    }

    private void Start()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    private void OnEnable()
    {
        GameEvents.OnTentacleAttackPlayer += OnPlayerAttacked;
        GameEvents.OnTentacleRetreat += OnTentacleRetreat;
    }

    private void OnDisable()
    {
        GameEvents.OnTentacleAttackPlayer -= OnPlayerAttacked;
        GameEvents.OnTentacleRetreat -= OnTentacleRetreat;
    }

    private void OnPlayerAttacked(TentacleController controller)
    {
        calculateDistance = true;
        this.controller = controller;
    }

    private void OnTentacleRetreat(TentacleController controller)
    {
        calculateDistance = false;
        this.controller = null;
    }

    private void Update()
    {
        if (calculateDistance)
        {
            distance = (controller.GetTentacleEndPoint() - transform.position).magnitude;
        }
        else
        {
            distance += Time.deltaTime * 2f;
        }

        paranoidAmount = Mathf.Clamp(1 - (distance * enemyDistanceMultiplier), 0.1f, 1f);
        currentHeartBeat = paranoidAmount * heartBeatMultiplier;
        currentShakeMagnitude = distance >= enemyDistanceThreshold ? 0 : (paranoidAmount * cameraShakeMultiplier);
        playerCamera.SetShakeMagnitude(currentShakeMagnitude);

        float t = (60f / currentHeartBeat);

        if (heartBeatTimer <= t)
            heartBeatTimer += Time.deltaTime;
        else
        {
            GameEvents.OnPlayAudio.Invoke("Heartbeat");

            heartBeatTimer = 0;
        }

    }
}
