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

    GameObject currentItem;
    GameObject spawnedPrompt = null;

    public void AddItem(GameObject item)
    {
        items.Add(item.GetComponent<ItemManager>().itemVariables);
    }

    public void RemoveItem(GameObject item)
    {
        items.Remove(item.GetComponent<ItemManager>().itemVariables);
    }

    void Start()
    {
        items = new List<ItemObjects>();
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
