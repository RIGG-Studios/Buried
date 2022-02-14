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
    public int maxSize;
    public EventSystem eventSystem;

    public GameObject connectorPrefab;

    public List<ItemObjects> itemConnectionsListA;
    public List<ItemObjects> itemConnectionsListB;

    public List<KnowledgeObject> connectionResultNotes;

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

        if (currentNote != null)
        {
            Vector2 mousePositionRelativeToViewportPosition = (Input.mousePosition - GetComponent<RectTransform>().position) / GetComponent<RectTransform>().rect.width * 2;
            Vector2 translatedMousePosition = new Vector2(viewportCamera.transform.position.x, viewportCamera.transform.position.y) + new Vector2(mousePositionRelativeToViewportPosition.x * viewportCamera.rect.width, mousePositionRelativeToViewportPosition.y * viewportCamera.rect.height) * viewportCamera.orthographicSize;

            currentNote.transform.position = new Vector3(translatedMousePosition.x, translatedMousePosition.y, currentNote.transform.position.z);
        }
        else
        {
            viewportCamera.transform.localPosition -= new Vector3(eventData.delta.x, eventData.delta.y, 0);
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        if(viewportCamera.orthographicSize > 0 && eventData.scrollDelta.y > 0)
        {
            viewportCamera.orthographicSize -= eventData.scrollDelta.y * scrollSpeed;
        }
        else if(viewportCamera.orthographicSize < maxSize && eventData.scrollDelta.y < 0)
        {
            viewportCamera.orthographicSize -= eventData.scrollDelta.y * scrollSpeed;
        }
    }

    private void Update()
    {
        viewportCamera.orthographicSize = Mathf.Clamp(viewportCamera.orthographicSize, 1, maxSize);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = RaycastFromMousePosition();

            if (hit)
            {
                currentNote = hit.collider.gameObject;
            }
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
            RaycastHit2D result = RaycastFromMousePosition();

            if(result && result.collider.gameObject != null && result.collider.gameObject.GetComponent<NoteManager>() != null)
            {
                NoteManager manager = result.collider.gameObject.GetComponent<NoteManager>();
                if (!manager.connected && inventory.connectors > 0)
                {
                    inventory.connectors--;
                    GameObject instantiatedConnector = Instantiate(connectorPrefab, result.collider.gameObject.transform);
                    instantiatedConnector.transform.position = new Vector3(result.point.x, result.point.y, instantiatedConnector.transform.position.z);

                    if (!isCurrentlyConnecting)
                    {
                        isCurrentlyConnecting = true;
                        currentNoteBeingConnnected = result.collider.gameObject;

                        manager.connected = true;
                    }
                    else
                    {
                        isCurrentlyConnecting = false;
                        int correspondingNoteIndex = GetCorrespondingNoteIndex();
                        ItemObjects correspondingNote;

                        if (IsNoteInListA())
                        {
                            correspondingNote = itemConnectionsListB[correspondingNoteIndex];
                        }
                        else if (IsNoteInListB())
                        {
                            correspondingNote = itemConnectionsListA[correspondingNoteIndex];
                        }
                        else
                        {
                            correspondingNote = null;
                        }

                        if (correspondingNote && correspondingNoteIndex > -1 && manager.noteVariables == correspondingNote && CanContinue(manager))
                        {
                            inventory.AddKnowledge(connectionResultNotes[correspondingNoteIndex]);

                            currentNoteBeingConnnected.GetComponent<NoteManager>().connectedTo.Add(manager);
                            manager.connectedTo.Add(currentNoteBeingConnnected.GetComponent<NoteManager>());
                        }
                        else
                        {
                            Destroy(currentNoteBeingConnnected.transform.GetChild(currentNoteBeingConnnected.transform.childCount - 1).gameObject);
                            Destroy(result.collider.gameObject.transform.GetChild(result.collider.gameObject.transform.childCount - 1).gameObject);
                        }

                        currentNoteBeingConnnected.GetComponent<NoteManager>().connected = false;
                        manager.connected = false;
                        currentNoteBeingConnnected = null;
                    }
                }
            }
        }
    }

    RaycastHit2D RaycastFromMousePosition()
    {
        Vector2 mousePositionRelativeToViewportPosition = (Input.mousePosition - GetComponent<RectTransform>().position)/GetComponent<RectTransform>().rect.width * 2;

        Vector2 translatedMousePosition = new Vector2(viewportCamera.transform.position.x, viewportCamera.transform.position.y) + new Vector2(mousePositionRelativeToViewportPosition.x * viewportCamera.rect.width, mousePositionRelativeToViewportPosition.y * viewportCamera.rect.height) * viewportCamera.orthographicSize;

        RaycastHit2D hit = Physics2D.Raycast(translatedMousePosition, -Vector3.forward, Mathf.Infinity);
        if (hit)
        {
            return hit;
        }

        return new RaycastHit2D();
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
            if (currentNoteBeingConnnected != null && itemConnectionsListB[v] == currentNoteBeingConnnected.GetComponent<NoteManager>().noteVariables)
            {
                return v;
            }
        }
        return -1;
    }

    bool IsNoteInListA()
    {
        for (int i = 0; i < itemConnectionsListA.Count; i++)
        {
            if (currentNoteBeingConnnected != null && itemConnectionsListA[i] == currentNoteBeingConnnected.GetComponent<NoteManager>().noteVariables)
            {
                return true;
            }
        }
        for (int v = 0; v < itemConnectionsListB.Count; v++)
        {
            if (currentNoteBeingConnnected != null && itemConnectionsListB[v] == currentNoteBeingConnnected.GetComponent<NoteManager>().noteVariables)
            {
                return false;
            }
        }

        return false;
    }

    bool IsNoteInListB()
    {
        for (int i = 0; i < itemConnectionsListA.Count; i++)
        {
            if (currentNoteBeingConnnected != null && itemConnectionsListA[i] == currentNoteBeingConnnected.GetComponent<NoteManager>().noteVariables)
            {
                return false;
            }
        }
        for (int v = 0; v < itemConnectionsListB.Count; v++)
        {
            if (currentNoteBeingConnnected != null && itemConnectionsListB[v] == currentNoteBeingConnnected.GetComponent<NoteManager>().noteVariables)
            {
                return true;
            }
        }

        return false;
    }

    bool CanContinue(NoteManager manager)
    {
        for (int i = 0; i < manager.connectedTo.Count; i++)
        {
            if (manager.connectedTo[i] == currentNoteBeingConnnected.GetComponent<NoteManager>())
            {
                return false;
            }
        }

        return true;
    }
}
