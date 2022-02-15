using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public KnowledgeObject key;
    public Sprite opened;
    public Collider2D colliderToDisable;

    public void Open(List<KnowledgeObject> knowledge)
    {
        for(int i = 0; i < knowledge.Count; i++)
        {
            if(knowledge[i] == key)
            {
                colliderToDisable.enabled = false;
                GetComponent<SpriteRenderer>().sprite = opened;
                transform.parent = transform.parent.parent;
            }
        }
    }
}
