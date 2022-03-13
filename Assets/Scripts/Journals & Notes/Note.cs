using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note 
{
    public string noteName;
    public Sprite noteContents;

    public Note(string noteName, Sprite noteContents)
    {
        this.noteName = noteName;
        this.noteContents = noteContents;
    }
}
