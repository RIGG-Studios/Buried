using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParanoidManager : MonoBehaviour
{
    [Range(0.1f, 1)]
    public float paranoidAmount;
    [Range(0, 10f)]
    public float enemyDistanceThreshold;
    [Range(0, 100f)]
    public float enemyDistanceMultiplier;

    public float heartBeatMultiplier = 1;
    public float cameraShakeMultiplier = 1;

    public AudioClip[] sounds;
    public AudioSource source;
    public PlayerCamera cam;

     float currentHeartBeat;
     float currentShakeMagnitude;
     float heartBeatTimer;

    private void Update()
    {
        float distance = (transform.position - GameObject.FindGameObjectWithTag("Enemy").transform.position).magnitude;

        currentShakeMagnitude = paranoidAmount >= 0.3f ? paranoidAmount * cameraShakeMultiplier : 0;
        if (distance >= enemyDistanceThreshold)
        {
            currentShakeMagnitude = 0;
        }


        paranoidAmount = Mathf.Clamp(1 - (distance * enemyDistanceMultiplier), 0.1f, 1f);

        currentHeartBeat = paranoidAmount * heartBeatMultiplier;
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
