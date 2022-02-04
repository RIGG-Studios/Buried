using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    public LayerMask interactionLayer;
    public GameObject interactionAssist;
    public float assistRadius;
    public float minInteractionDistance;

    Player player;
    public InteractableObject hoveredObject { get; private set; }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition());
        Vector3 worldPos = mouseWorldSpace;

        RaycastHit2D hit = Physics2D.CircleCast(worldPos, 0.5f, transform.position - worldPos, minInteractionDistance, interactionLayer);

        if (hit.collider != null)
        {
            hoveredObject = hit.collider.GetComponent<InteractableObject>();

            if (hoveredObject.canInteract)
            {
                interactionAssist.SetActive(true);
            }
        }
        else if(hit.collider == null || !hoveredObject.canInteract)
        {
            hoveredObject = null;
            interactionAssist.SetActive(false);
        }
    }

    public void InteractWithObject()
    {
        hoveredObject.Interact();
        interactionAssist.SetActive(false);
    }

    public void UpdateInteractionAssistRotation(Vector3 direction) => interactionAssist.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
}
