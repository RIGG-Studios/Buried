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


    private void Update()
    {
        controller = Game.instance.tentacleManager.GetClosestTentacleToPlayer(transform.position);

        if (controller == null)
        {
            playerCamera.SetShakeMagnitude(0.0f);
            distance /= Time.deltaTime * 2f;
        }
        else
        {
            distance = (controller.GetTentacleEndPoint() - transform.position).magnitude;

            currentShakeMagnitude = distance >= enemyDistanceThreshold ? Mathf.Lerp(currentShakeMagnitude, 0, Time.deltaTime * 5f) : (paranoidAmount * cameraShakeMultiplier);
            playerCamera.SetShakeMagnitude(currentShakeMagnitude);
        }

        paranoidAmount = Mathf.Clamp(1 - (distance * enemyDistanceMultiplier), 0.1f, 1f);
        currentHeartBeat = paranoidAmount * heartBeatMultiplier;


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
