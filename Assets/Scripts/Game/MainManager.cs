using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainManager: MonoBehaviour
{
    [SerializeField]
    int notesInLevel;

    static bool canOpenDoor;
    static int maxNotes;

    private void Start()
    {
        maxNotes = notesInLevel;
    }

    public static int GetRemainingNotes(ItemDatabase inventory)
    {
        Item notes = inventory.FindItem(ItemProperties.ItemTypes.Note);

        if(notes != null)
        {
            if(maxNotes - notes.stack <= 0)
            {
                canOpenDoor = true;
            }

            return maxNotes - notes.stack;
        }

        return maxNotes;
    }

    public static int GetMaxNotes => maxNotes;

    public static bool GetCanOpenDoor => canOpenDoor;
}
