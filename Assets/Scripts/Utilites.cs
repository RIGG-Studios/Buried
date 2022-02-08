using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Utilites : MonoBehaviour
{
    static GameObject player;
    public static Vector3 GetPlayerPosition()
    {
        if(player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(p);
            player = p;
        }

        return player.transform.position;
    }

    public static Player GetPlayer()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            player = p;
        }

        return player.GetComponent<Player>();
    }

    public static Vector2 GetMousePosition() => Mouse.current.position.ReadValue();

    public static RaycastResult IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    private static RaycastResult IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("Interaction"))
                return curRaysastResult;
        }

        return new RaycastResult();
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = GetMousePosition();
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
