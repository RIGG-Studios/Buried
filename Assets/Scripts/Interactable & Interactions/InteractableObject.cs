using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InteractionType
{
    Search,
    Hide,
    Loot,
    Activate,
}

public abstract class InteractableObject : MonoBehaviour
{
    public bool showTutorial;
    public bool useAssist;
    public bool interactable;
    public Text tutorialText;

    private void Start()
    {
        if (tutorialText == null)
            return;

        if (showTutorial)
            tutorialText.gameObject.SetActive(true);
        else 
            tutorialText.gameObject.SetActive(false);

    }

    public abstract void HoverInteract();
    public abstract void StopHoverInteract();
    public abstract void ButtonInteract();
}
