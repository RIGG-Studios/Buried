using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManipulateWindow : MonoBehaviour, IDragHandler, IScrollHandler
{
    public PersistentData viewportData;
    public PlayerInventory inventory;
    public float scrollSpeed;
    public EventSystem eventSystem;

    public GameObject connectorPrefab;

    public List<ItemObjects> itemConnectionsListA;
    public List<ItemObjects> itemConnectionsListB;

    public List<GameObject> connectionResultNotes;

    GameObject currentNote;
    GameObject currentNoteBeingConnnected;

    GraphicRaycaster rayCaster;

    Camera viewportCamera;
    bool isCurrentlyConnecting;

    void Start()
    {
        viewportCamera = viewportData.viewportCamera;
        rayCaster = viewportData.viewportCanvas.GetComponent<GraphicRaycaster>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 viewportCameraPos = viewportCamera.transform.position;

        if(currentNote != null)
        {
            currentNote.GetComponent<RectTransform>().anchoredPosition += eventData.delta / transform.parent.GetComponent<Canvas>().scaleFactor;
        }
        else
        {
            viewportCamera.transform.position = new Vector3(viewportCameraPos.x - eventData.delta.x, viewportCameraPos.y - eventData.delta.y, viewportCameraPos.z);
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        viewportCamera.orthographicSize -= eventData.scrollDelta.y * scrollSpeed;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            currentNote = RaycastFromMousePosition().gameObject;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (currentNote != null)
            {
                currentNote = null;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastResult result = RaycastFromMousePosition();

            if(result.gameObject != null && result.gameObject.GetComponent<NoteManager>() != null)
            {
                NoteManager manager = result.gameObject.GetComponent<NoteManager>();
                if (!manager.connected)
                {
                    GameObject instantiatedConnector = Instantiate(connectorPrefab, result.gameObject.transform);
                    instantiatedConnector.transform.position = result.worldPosition;

                    if (!isCurrentlyConnecting)
                    {
                        isCurrentlyConnecting = true;
                        currentNoteBeingConnnected = result.gameObject;

                        manager.connected = true;
                    }
                    else
                    {
                        isCurrentlyConnecting = false;
                        int correspondingNoteIndex = GetCorrespondingNoteIndex();
                        ItemObjects correspondingNote = itemConnectionsListB[correspondingNoteIndex];
                        if(correspondingNote == null)
                        {
                            correspondingNote = itemConnectionsListA[correspondingNoteIndex];
                        }

                        if (correspondingNote && manager.noteVariables == correspondingNote)
                        {
                            inventory.AddItem(connectionResultNotes[correspondingNoteIndex]);
                        }
                        else
                        {
                            Destroy(currentNoteBeingConnnected.transform.GetChild(currentNoteBeingConnnected.transform.childCount - 1).gameObject);
                            Destroy(result.gameObject.transform.GetChild(result.gameObject.transform.childCount - 1).gameObject);
                        }

                        currentNoteBeingConnnected.GetComponent<NoteManager>().connected = false;
                        manager.connected = false;
                        currentNoteBeingConnnected = null;
                    }
                }
            }
        }
    }

    RaycastResult RaycastFromMousePosition()
    {
        Vector2 worldPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out worldPoint);

        Vector2 vector = worldPoint + new Vector2(viewportCamera.transform.position.x, viewportCamera.transform.position.y);
        Vector2 finalVector = viewportCamera.WorldToScreenPoint(vector);

        PointerEventData pointerEventData = new PointerEventData(eventSystem);

        pointerEventData.position = finalVector;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        rayCaster.Raycast(pointerEventData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            if (currentNote == null)
            {
                return result;
            }
        }

        return new RaycastResult();
    }

    int GetCorrespondingNoteIndex()
    {
        for (int i = 0; i < itemConnectionsListA.Count; i++)
        {
            if (currentNoteBeingConnnected != null && itemConnectionsListA[i] == currentNoteBeingConnnected.GetComponent<NoteManager>().noteVariables)
            {
                return i;
            }
        }
        for(int v = 0; v < itemConnectionsListB.Count; v++)
        {
            if (currentNoteBeingConnnected != null && itemConnectionsListA[v] == currentNoteBeingConnnected.GetComponent<NoteManager>().noteVariables)
            {
                return v;
            }
        }
        return 0;
    }
}
