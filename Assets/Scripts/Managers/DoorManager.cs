using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public KnowledgeObject key;
    public Sprite opened;

    public void Open(List<KnowledgeObject> knowledge)
    {
        for(int i = 0; i < knowledge.Count; i++)
        {
            if(knowledge[i] == key)
            {
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().sprite = opened;
                transform.parent = transform.parent.parent;
            }
        }
    }
}
