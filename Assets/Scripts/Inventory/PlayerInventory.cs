using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemObjects> items;
    public int collectionRange;

    public Transform itemRepository;
    public Transform doorRepository;
    public GameObject pickupPrompt;
    public Vector3 offset;

    public PersistentData viewportData;
    public TextMeshProUGUI knowledgePopup;

    GameObject currentItem;
    GameObject spawnedPrompt = null;

    Vector2 currentNoteOffset;

    Transform viewportNotes;

    bool debounce;

    public GameObject AddItem(ItemObjects item, GameObject prefabNote)
    {
        GameObject newNote = null;

        if (item != null)
        {
            items.Add(item);
            newNote = Instantiate(prefabNote, viewportNotes);

            newNote.GetComponent<NoteManager>().noteVariables = item;
        }
        if(newNote != null)
        {
            newNote.transform.position = new Vector3(viewportNotes.position.x, viewportNotes.position.y, (viewportData.noteRepository.childCount - 1) * 0.1f);
            newNote.transform.position += new Vector3(currentNoteOffset.x, currentNoteOffset.y, newNote.transform.position.z);

            currentNoteOffset += new Vector2(20, -20);
        }

        return newNote;
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item.GetComponent<ItemManager>().itemVariables);
    }

    IEnumerator DisplayKnowledgePopUp()
    {
        if (!debounce)
        {
            debounce = true;
            knowledgePopup.gameObject.SetActive(true);

            yield return new WaitForSeconds(5);

            knowledgePopup.gameObject.SetActive(false);
            debounce = false;
        }
    }

    void Start()
    {
        items = new List<ItemObjects>();
        viewportNotes = viewportData.noteRepository;
    }

    void Update()
    {
        /*
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
        */

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit;

            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity);

            if (hit)
            {
                if(hit.transform.parent == itemRepository || hit.transform.parent == doorRepository)
                {
                    if((transform.position - hit.transform.position).magnitude <= collectionRange)
                    {
                        currentItem = hit.transform.gameObject;
                    }
                    else
                    {
                        knowledgePopup.text = "That object is too far away";
                        StartCoroutine("DisplayKnowledgePopUp");
                    }
                }
            }

            if(currentItem != null)
            {
                if(currentItem.GetComponent<ItemManager>() != null)
                {
                    AddItem(currentItem.GetComponent<ItemManager>().itemVariables, currentItem.GetComponent<ItemManager>().itemPrefab);

                    Destroy(currentItem);
                    currentItem = null;
                }
                else if(currentItem.GetComponent<DoorManager>() != null)
                {
                    currentItem.GetComponent<DoorManager>().Open(items);
                    currentItem = null;
                }
            }
        }
    }

    public void StopKnowledgeTextCoroutine()
    {
        StopCoroutine(DisplayKnowledgePopUp());
        debounce = false;
    }
}
