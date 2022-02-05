using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManipulateWindow : MonoBehaviour, IDragHandler, IScrollHandler, IPointerUpHandler, IPointerDownHandler
{
    public PersistentData viewportData;
    public float scrollSpeed;

    Camera viewportCamera;

    void Start()
    {
        viewportCamera = viewportData.viewportCamera;
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
        Debug.Log("Released click");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Started click");
    }
}
