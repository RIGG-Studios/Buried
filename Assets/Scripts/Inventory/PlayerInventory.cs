using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemObjects> items;
    public List<KnowledgeObject> knowledge;
    public int collectionRange;

    public Transform itemRepository;
    public Transform doorRepository;
    public GameObject pickupPrompt;
    public Vector3 offset;

    public PersistentData viewportData;
    public GameObject prefabNote;
    public TextMeshProUGUI knowledgePopup;
    public TextMeshProUGUI connectorCount;

    public int connectors;

    GameObject currentItem;
    GameObject spawnedPrompt = null;

    Vector2 currentNoteOffset;

    Transform viewportNotes;

    public void AddItem(GameObject item)
    {
        GameObject newNote = null;

        if (item.GetComponent<ItemManager>() != null)
        {
            items.Add(item.GetComponent<ItemManager>().itemVariables);
            newNote = Instantiate(prefabNote, viewportNotes);

            newNote.GetComponent<NoteManager>().noteVariables = item.GetComponent<ItemManager>().itemVariables;
        }
        else if (item.GetComponent<NoteManager>() != null)
        {
            items.Add(item.GetComponent<NoteManager>().noteVariables);
            newNote = Instantiate(prefabNote, viewportNotes);

            newNote.GetComponent<NoteManager>().noteVariables = item.GetComponent<NoteManager>().noteVariables;
        }
        else if(item.name == "Connector")
        {
            connectors+=2;
        }

        if(item.name != "Connector" && newNote != null)
        {
            newNote.transform.position = new Vector3(viewportNotes.position.x, viewportNotes.position.y, newNote.transform.position.z);
            newNote.transform.position += new Vector3(currentNoteOffset.x, currentNoteOffset.y, newNote.transform.position.z);

            currentNoteOffset += new Vector2(20, -20);
        }
    }

    public void AddKnowledge(KnowledgeObject newKnowledge)
    {
        knowledge.Add(newKnowledge);
        knowledgePopup.text = "You have gained knowledge of " + newKnowledge.description;
        StartCoroutine("DisplayKnowledgePopUp");
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item.GetComponent<ItemManager>().itemVariables);
    }

    IEnumerator DisplayKnowledgePopUp()
    {
        knowledgePopup.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        knowledgePopup.gameObject.SetActive(false);
    }

    void Start()
    {
        items = new List<ItemObjects>();
        viewportNotes = viewportData.noteRepository;
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
                spawnedPrompt = Instantiate(pickupPrompt);
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
                if(currentItem.GetComponent<ItemManager>() != null || currentItem.name == "Connector")
                {
                    AddItem(currentItem);

                    Destroy(currentItem);
                    currentItem = null;
                }
                else if(currentItem.GetComponent<DoorManager>() != null)
                {
                    currentItem.GetComponent<DoorManager>().Open(knowledge);
                    currentItem = null;
                }
            }
        }

        connectorCount.text = "Connectors - " + connectors;
    }
}
