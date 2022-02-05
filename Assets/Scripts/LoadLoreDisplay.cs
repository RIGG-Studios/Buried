using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLoreDisplay : MonoBehaviour
{
    public PersistentData data;
    public Camera mainCamera;

    void Awake()
    {
        data.viewportCamera = mainCamera;    
    }
}
