using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private List<PlayerFootsteps> footSteps = new List<PlayerFootsteps>();

    public FootstepAudio footStepAudio = null;

    private AudioSource source;


    public void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        source.volume = footStepAudio.volume;
        source.PlayOneShot(footStepAudio.GetRandomClip());
    }
}
