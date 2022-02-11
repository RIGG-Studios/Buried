using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemObjects> items;
    public List<KnowledgeObject> knowledge;
    public int collectionRange;

    public Transform itemRepository;
    public Transform doorRepository;
    public GameObject pickupPrompt;
    public Vector3 offset;
    public Canvas loreCorkboard;

    public PersistentData viewportData;
    public GameObject prefabNote;

    public int connectors;

    GameObject currentItem;
    GameObject spawnedPrompt = null;

    Vector2 currentNoteOffset;

    Canvas viewportCanvas;

    public void AddItem(GameObject item)
    {
        GameObject newNote = Instantiate(prefabNote, viewportCanvas.transform);

        if (item.GetComponent<ItemManager>() != null)
        {
            items.Add(item.GetComponent<ItemManager>().itemVariables);
            newNote.GetComponent<NoteManager>().noteVariables = item.GetComponent<ItemManager>().itemVariables;
        }
        else if (item.GetComponent<NoteManager>() != null)
        {
            items.Add(item.GetComponent<NoteManager>().noteVariables);
            newNote.GetComponent<NoteManager>().noteVariables = item.GetComponent<NoteManager>().noteVariables;
        }
        else if(item.name == "Connector")
        {
            connectors+=2;
        }

        newNote.transform.position = new Vector3(viewportCanvas.transform.position.x, viewportCanvas.transform.position.y, newNote.transform.position.z);
        newNote.transform.position += new Vector3(currentNoteOffset.x, currentNoteOffset.y, newNote.transform.position.z);

        currentNoteOffset += new Vector2(newNote.transform.localScale.x * 20, -newNote.transform.localScale.y * 20);
    }

    public void AddKnowledge(KnowledgeObject newKnowledge)
    {
        knowledge.Add(newKnowledge);
        Debug.Log("I have gained new knowledge");
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item.GetComponent<ItemManager>().itemVariables);
    }

    void Start()
    {
        items = new List<ItemObjects>();
        viewportCanvas = viewportData.viewportCanvas;
    }

    void Update()
    {
        for (int i = 0; i < itemRepository.childCount; i++)
        {
            if (((transform.position - itemRepository.GetChild(i).position).magnitude <= collectionRange))
            {
                currentItem = itemRepository.GetChild(i).gameObject;
                break;
            }

            currentItem = null;
        }

        if(currentItem == null)
        {
            for (int v = 0; v < doorRepository.childCount; v++)
            {
                if (((transform.position - doorRepository.GetChild(v).position).magnitude <= collectionRange))
                {
                    currentItem = doorRepository.GetChild(v).gameObject;
                    break;
                }

                currentItem = null;
            }
        }

        if(currentItem != null)
        {
            if(spawnedPrompt == null)
            {
                spawnedPrompt = Instantiate(pickupPrompt, currentItem.transform);
            }
            spawnedPrompt.transform.position = currentItem.transform.position + offset;
        }
        else
        {
            if(spawnedPrompt != null)
            {
                Destroy(spawnedPrompt);
                spawnedPrompt = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(currentItem != null)
            {
                if(currentItem.GetComponent<ItemManager>() != null)
                {
                    AddItem(currentItem);

                    Destroy(currentItem);
                    currentItem = null;
                }
                else if(currentItem.GetComponent<DoorManager>() != null)
                {
                    currentItem.GetComponent<DoorManager>().Open(knowledge);
                }
            }
        }
    }
}
