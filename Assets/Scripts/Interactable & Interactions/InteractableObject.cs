using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Search,
    Hide,
    Loot,
    Activate,
}

public abstract class InteractableObject : MonoBehaviour
{
    public bool useAssist;
    public bool interactable;

    public abstract void HoverInteract();
    public abstract void StopHoverInteract();
    public abstract void ButtonInteract();
}
