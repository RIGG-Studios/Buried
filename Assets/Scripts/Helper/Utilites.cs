using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Utilites : MonoBehaviour
{
    public static Vector2 GetMousePosition() => Mouse.current.position.ReadValue();

    public static RaycastResult IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    public static RaycastResult IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("Interaction"))
                return curRaysastResult;
        }

        return new RaycastResult();
    }

    public static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = GetMousePosition();

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        Vector2 normDir = dir.normalized;
        float step = 360f / sliceCount;
        float halfstep = step / 2;
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        angle += halfstep;

        if (angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }

    public static float NormalizeToRange(float value, float max, float min)
    {
        float normalized = (value - min) / (max - min);

        return normalized;
    }


    public static float NormalizeAngle(float radians)
    {
        while (radians < 0)
            radians += Mathf.PI * 2f;

        while (radians >= Mathf.PI * 2)
            radians -= Mathf.PI * 2;

        return radians;
    }
}
