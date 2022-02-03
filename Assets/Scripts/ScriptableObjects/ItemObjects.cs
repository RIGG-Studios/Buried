using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Item")]
public class ItemObjects : ScriptableObject
{
    public bool isClue;
    public string toolTip;
    new public string name;
    public Sprite sprite;
}
