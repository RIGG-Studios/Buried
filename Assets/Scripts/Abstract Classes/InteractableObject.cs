using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public bool canInteract;
    public List<AudioClip> interactAudio = new List<AudioClip>();

    public abstract void Interact();
}
