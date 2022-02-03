using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDragging : MonoBehaviour
{
    bool holdingMouseButton;
    ItemManager currentHeldItem;
    Vector2 mousePosition;

    public Camera mainCamera;
    public float interactRange;

    void Start()
    {

    }

    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            holdingMouseButton = true;
            RaycastHit2D hit = RaycastFromMousePosition();

            if (hit && hit.collider.GetComponent<ItemManager>() && currentHeldItem == null && (transform.position - hit.collider.transform.position).magnitude <= interactRange)
            {
                currentHeldItem = hit.collider.GetComponent<ItemManager>();
                currentHeldItem.OnPickUp(mousePosition, transform);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            holdingMouseButton = false;

            if (currentHeldItem)
            {
                currentHeldItem.OnDrop(mousePosition, transform, interactRange);
                currentHeldItem = null;
            }
        }

        if(holdingMouseButton && currentHeldItem != null)
        {
            currentHeldItem.Tick(mousePosition, transform);
        }
    }

    RaycastHit2D RaycastFromMousePosition()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition, mainCamera.transform.forward, Mathf.Infinity);

        return raycastHit;
    }
}
