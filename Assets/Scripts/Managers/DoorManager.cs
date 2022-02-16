using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public ItemObjects key;
    public Sprite opened;
    public Collider2D colliderToDisable;

    public void Open(List<ItemObjects> items)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i] == key)
            {
                colliderToDisable.enabled = false;
                GetComponent<SpriteRenderer>().sprite = opened;
                transform.parent = transform.parent.parent;
            }
        }
    }
}
