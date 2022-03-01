using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private GameObject interactionAssist;
    [SerializeField] private TextMeshPro interactionName;
    [SerializeField] private TextMeshPro interactionType;

    [SerializeField] private float assistRadius = 0.0f;
    [SerializeField] private float minInteractionDistance = 0.0f;

    [HideInInspector]
    public InteractableObject hoveredObject;

    private Player player = null;
    private Camera camera = null;

    private void Awake()
    {
        player = GetComponent<Player>();
        camera = Camera.main;
    }

    private void Start()
    {
        player.input.Player.Flashlight.performed += ctx => InteractWithObject();
    }

    private void Update()
    {
        Vector2 mouseWorldSpace = camera.ScreenToWorldPoint(Utilites.GetMousePosition());
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
        Debug.Log("hi");
        hoveredObject.Interact(player);
        interactionAssist.SetActive(false);
    }


    public void UpdateInteractionAssistRotation(Vector3 direction)
    {
        interactionAssist.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
