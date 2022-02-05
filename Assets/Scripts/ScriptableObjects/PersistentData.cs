using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New data", menuName = "Persistent Data")]
public class PersistentData : ScriptableObject
{
    public Camera viewportCamera;
    public Canvas viewportCanvas;
}
