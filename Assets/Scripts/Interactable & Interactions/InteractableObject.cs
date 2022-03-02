using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Search,
    Hide,
    Loot
}

public abstract class InteractableObject : MonoBehaviour
{
    public bool useAssist;
    public bool open;
    public string interactionName;
    public InteractionType interactionType;

    public abstract void HoverInteract();
    public abstract void StopHoverInteract();
    public abstract void ButtonInteract();
}
