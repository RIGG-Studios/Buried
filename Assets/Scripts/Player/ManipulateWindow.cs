using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManipulateWindow : MonoBehaviour, IDragHandler, IScrollHandler, IPointerUpHandler, IPointerDownHandler
{
    public PersistentData viewportData;
    public float scrollSpeed;
    public EventSystem eventSystem;

    GraphicRaycaster rayCaster;

    Camera viewportCamera;

    void Start()
    {
        viewportCamera = viewportData.viewportCamera;
        rayCaster = viewportData.viewportCanvas.GetComponent<GraphicRaycaster>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 viewportCameraPos = viewportCamera.transform.position;

        viewportCamera.transform.position = new Vector3(viewportCameraPos.x - eventData.delta.x, viewportCameraPos.y - eventData.delta.y, viewportCameraPos.z);
    }

    public void OnScroll(PointerEventData eventData)
    {
        viewportCamera.orthographicSize -= eventData.scrollDelta.y * scrollSpeed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 worldPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out worldPoint);

        Vector2 vector = worldPoint + new Vector2(viewportCamera.transform.position.x, viewportCamera.transform.position.y);
        Vector2 finalVector = viewportCamera.WorldToScreenPoint(vector);

        Debug.Log(finalVector);

        PointerEventData pointerEventData = new PointerEventData(eventSystem);

        pointerEventData.position = finalVector;

        List <RaycastResult> raycastResults = new List<RaycastResult>();
        rayCaster.Raycast(pointerEventData, raycastResults);

        foreach(RaycastResult result in raycastResults)
        {
            Debug.Log(result.gameObject.transform.name);
        }

    }
}
