using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLoreDisplay : MonoBehaviour
{
    public PersistentData data;
    public Camera mainCamera;
    public Canvas viewportCanvas;
    public NoteLineRenderer lineRenderer;
    void Awake()
    {
        data.viewportCamera = mainCamera;
        data.viewportCanvas = viewportCanvas;
        data.lineRenderer = lineRenderer;
    }
}
