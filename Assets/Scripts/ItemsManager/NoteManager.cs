using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    Canvas canvas;

    public void OnDrag(Vector2 mouseDelta)
    {
        transform.GetComponent<RectTransform>().anchoredPosition += mouseDelta / canvas.scaleFactor;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.parent.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
