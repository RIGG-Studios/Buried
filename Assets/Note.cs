using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note 
{
    public string author;
    public string date;
    public string header;
    public string description;
    public Note(string author, string date, string header, string description)
    {
        this.author = author;
        this.date = date;
        this.header = header;
        this.description = description;
    }
}
