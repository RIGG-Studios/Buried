using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerInteractionManager : MonoBehaviour
{
    public LayerMask interactionLayer;

    public GameObject interactionAssist;
    public TextMeshPro interactionName;
    public TextMeshPro interactionType;

    public float assistRadius;
    public float minInteractionDistance;

    public InteractableObject hoveredObject;

    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition());
        Vector3 worldPos = mouseWorldSpace;

        RaycastHit2D spriteHit = Physics2D.Raycast(worldPos, transform.position - worldPos, minInteractionDistance, interactionLayer);
        RaycastResult uiHit = Utilites.IsPointerOverUIElement();

        if (uiHit.gameObject != null)
        {
            OnHoverOverInteractable(uiHit.gameObject, Vector2.zero);
        }
        else if (uiHit.gameObject == null && hoveredObject != null && spriteHit.collider == null)
            OnStopHoverInteractable();

        if (spriteHit.collider != null)
        {
            OnHoverOverInteractable(spriteHit.collider.gameObject, spriteHit.point);
        }
        else if(spriteHit.collider == null && hoveredObject != null && uiHit.gameObject == null)
            OnStopHoverInteractable();
    }

    private void OnHoverOverInteractable(GameObject collision, Vector2 point)
    {
        hoveredObject = collision.GetComponent<InteractableObject>();

        if (hoveredObject.useAssist)
        {
            interactionName.text = hoveredObject.interactionName;
            interactionType.text = hoveredObject.interactionType.ToString();
            interactionAssist.transform.position = point;
            interactionAssist.SetActive(true);
        }
    }

    private void OnStopHoverInteractable()
    {
        hoveredObject.StopInteract();
        hoveredObject = null;

        interactionAssist.SetActive(false);
    }

    public void InteractWithObject()
    {
        hoveredObject.Interact(player);
        interactionAssist.SetActive(false);
    }


    public void UpdateInteractionAssistRotation(Vector3 direction) => interactionAssist.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
}
