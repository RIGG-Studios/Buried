using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParanoidManager : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1)]
    private float paranoidAmount;
    [SerializeField, Range(0, 100f)]
    private float enemyDistanceThreshold;
    [SerializeField, Range(0, 100f)]
    private float enemyDistanceMultiplier;

    public float heartBeatMultiplier = 1;
    public float cameraShakeMultiplier = 1;

    public AudioClip[] sounds;
    public AudioSource source;
    public PlayerCamera cam;

    float currentHeartBeat = 0.0f;
    float currentShakeMagnitude = 0.0f;
    float heartBeatTimer = 0.0f;
    float distance = 0.0f;

    bool calculateDistance;
    TentacleController controller;

    private void Awake()
    {
        distance = 100f;
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
        cam.SetShakeMagnitude(currentShakeMagnitude);

        float t = (60f / currentHeartBeat);

        if (heartBeatTimer <= t)
            heartBeatTimer += Time.deltaTime;
        else
        {
            foreach (AudioClip c in sounds)
                source.PlayOneShot(c);

            heartBeatTimer = 0;
        }

    }
}
