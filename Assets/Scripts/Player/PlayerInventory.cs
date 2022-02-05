using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemObjects> items;
    public int collectionRange;

    public Transform itemRepository;
    public GameObject pickupPrompt;
    public Vector3 offset;
    public Canvas loreCorkboard;

    public PersistentData viewportData;
    public GameObject prefabNote;

    GameObject currentItem;
    GameObject spawnedPrompt = null;

    Vector2 currentNoteOffset;

    Canvas viewportCanvas;

    public void AddItem(GameObject item)
    {
        if(item.GetComponent<ItemManager>() != null)
        {
            items.Add(item.GetComponent<ItemManager>().itemVariables);
        }
        else if (item.GetComponent<NoteManager>() != null)
        {
            items.Add(item.GetComponent<NoteManager>().noteVariables);
        }

        GameObject newNote = Instantiate(prefabNote, viewportCanvas.transform);
        newNote.transform.position += new Vector3(currentNoteOffset.x, currentNoteOffset.y, newNote.transform.position.z);

        currentNoteOffset += new Vector2(newNote.transform.localScale.x/2, newNote.transform.localScale.y/2);
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
            if ((transform.position - itemRepository.GetChild(i).position).magnitude <= collectionRange)
            {
                currentItem = itemRepository.GetChild(i).gameObject;
                break;
            }

            currentItem = null;
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
                AddItem(currentItem);

                Destroy(currentItem);
                currentItem = null;
            }
        }
    }
}
