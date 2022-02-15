using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Item")]
public class ItemObjects : ScriptableObject
{
    new public string name;
    [TextArea]
    public string text;

    public KnowledgeObject knowledge;
}
