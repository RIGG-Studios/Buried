using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParanoidManager : MonoBehaviour
{
    [Range(0.1f, 1)]
    public float paranoidAmount;
    [Range(0, 100f)]
    public float enemyDistanceThreshold;
    [Range(0, 100f)]
    public float enemyDistanceMultiplier;

    public float heartBeatMultiplier = 1;
    public float cameraShakeMultiplier = 1;

    public AudioClip[] sounds;
    public AudioSource source;
    public PlayerCamera cam;

     float currentHeartBeat;
     public float currentShakeMagnitude;
     float heartBeatTimer;
    private void Update()
    {
        float distance = (Game.instance.monster.GetClosestTentacleToPlayer().GetTentacleEndPoint() - transform.position).magnitude;
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
