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
        Vector3 worldPos = player.GetMousePositionInWorldSpace();

        RaycastHit2D hit = Physics2D.CircleCast(worldPos, 0.5f, transform.position - worldPos, Mathf.Infinity, interactionLayer);

        if (hit.collider != null)
        {
            float dist = (hit.collider.transform.position - transform.position).magnitude;

            if (dist >= minInteractionDistance)
                return;

            hoveredObject = hit.collider.GetComponent<InteractableObject>();

            if (hoveredObject.canInteract)
            {
                interactionAssist.transform.position = hit.point;
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

}
