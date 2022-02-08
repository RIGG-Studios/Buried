using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Search,
    Loot,
    Examine,
    Hide,
    UnHide
}

public abstract class InteractableObject : MonoBehaviour
{

    public bool useAssist;
    public bool draggable;
    public string interactionName;
    public InteractionType interactionType;
    public List<AudioClip> interactAudio = new List<AudioClip>();

    public abstract void Interact(Player player);

    public abstract void StopInteract();
}
