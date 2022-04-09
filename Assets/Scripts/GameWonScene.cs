using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWonScene : MonoBehaviour
{
    private CanvasManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<CanvasManager>();
    }
}
