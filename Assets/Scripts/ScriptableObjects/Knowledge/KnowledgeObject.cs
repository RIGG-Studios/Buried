using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Knowledge", menuName = "Knowledge")]
public class KnowledgeObject : ScriptableObject
{
    public new string name;
    public string description;
}
