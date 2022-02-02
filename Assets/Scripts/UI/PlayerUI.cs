using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject map;

    bool mapShown;

    public void ToggleMap()
    {
        if (mapShown)
        {
            map.SetActive(false);
            mapShown = false;
        }
        else if (!mapShown)
        {
            map.SetActive(true);
            mapShown = true;
        }
    }
}
