using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLoreDisplay : MonoBehaviour
{
    public PersistentData data;
    public Camera mainCamera;
    public Canvas viewportCanvas;
    public Transform noteRepository;
    public NoteLineRenderer lineRenderer;

    void Awake()
    {
        data.viewportCamera = mainCamera;
        data.viewportCanvas = viewportCanvas;
        data.lineRenderer = lineRenderer;
        data.noteRepository = noteRepository;
    }

    private void Start()
    {
        mainCamera.transparencySortMode = TransparencySortMode.Orthographic;
    }
}
