using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public ItemObjects key;
    public Sprite opened;

    public void Open(List<ItemObjects> notes)
    {
        for(int i = 0; i < notes.Count; i++)
        {
            if(notes[i] == key)
            {
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().sprite = opened;
                transform.parent = transform.parent.parent;
            }
        }
    }
}
