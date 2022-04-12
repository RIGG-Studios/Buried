using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D hoverCursor;

    [SerializeField] private float assistRadius = 0.0f;
    [SerializeField] private float minInteractionDistance = 0.0f;


    [HideInInspector]
    public InteractableObject hoveredObject;
    [HideInInspector]
    public bool allowInteractions;

    private Player player = null;
    private Camera camera = null;

    private void Awake()
    {
        player = GetComponent<Player>();
        camera = Camera.main;
    }

    private void Start()
    {
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

        player.playerInput.Player.Interact.performed += ctx => InteractWithObject();
        allowInteractions = true;
    }


    private void Update()
    {
        Ray ray = camera.ScreenPointToRay(Utilites.GetMousePosition());

        RaycastHit2D spriteHit = Physics2D.GetRayIntersection(ray, 1500f, interactionLayer);

        if (allowInteractions)
        {
            if (spriteHit.collider != null)
            {
                float dist = (spriteHit.collider.transform.position - transform.position).magnitude;

                if (dist < 2)
                    OnHoverOverInteractable(spriteHit.collider.gameObject, spriteHit.point);
            }
            else if (spriteHit.collider == null && hoveredObject != null)
            {
                OnStopHoverInteractable();
            }
        }
    }

    private void OnHoverOverInteractable(GameObject collision, Vector2 point)
    {
        hoveredObject = collision.GetComponent<InteractableObject>();

        if (hoveredObject.useAssist && hoveredObject.interactable)
        {
            Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }

        hoveredObject.HoverInteract();
    }

    private void OnStopHoverInteractable()
    {
        if (hoveredObject == null)
            return;

        hoveredObject.StopHoverInteract();
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);   
        hoveredObject = null;
    }

    public void InteractWithObject()
    {
        if (hoveredObject == null || !hoveredObject.interactable)
            return;

        hoveredObject.ButtonInteract();
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
