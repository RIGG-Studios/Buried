using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteManager : MonoBehaviour
{
    public bool connected;
    public List<NoteManager> connectedTo;
    public ItemObjects noteVariables;

    public TextMeshProUGUI title;
    public TextMeshProUGUI body;

    void Start()
    {
        connected = false;
        title.text = noteVariables.name;
        body.text = noteVariables.text;
    }
}
