using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionManager : MonoBehaviour
{
    public LayerMask interactionLayer;
    public GameObject interactionAssist;
    public float assistRadius;
    public float minInteractionDistance;

    Player player;
    public InteractableObject hoveredObject;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition());
        Vector3 worldPos = mouseWorldSpace;

        RaycastHit2D hit = Physics2D.CircleCast(worldPos, 0.5f, transform.position - worldPos, minInteractionDistance, interactionLayer);
        RaycastResult result = Utilites.IsPointerOverUIElement();

        GameObject collision = null;
        if (hit)
            collision = hit.collider.gameObject;
        if (result.gameObject)
            collision = result.gameObject;

        if (collision != null)
        {
            hoveredObject = collision.GetComponent<InteractableObject>();

            if (hoveredObject.useAssist)
            {
                interactionAssist.SetActive(true);
            }
        }
        else if(hit.collider == null && hoveredObject != null)
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
