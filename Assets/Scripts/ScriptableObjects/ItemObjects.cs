using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Item")]
public class ItemObjects : ScriptableObject
{
    new public string name;
    public string text;
    public Sprite sprite;
}
