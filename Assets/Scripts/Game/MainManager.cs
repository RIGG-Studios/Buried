using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MainManager: MonoBehaviour
{
    [SerializeField]
    int notesInLevel;

    [SerializeField]
    string nextSceneName;

    static bool canOpenDoor;
    static int maxNotes;

    public static string nextScene;

    private void Start()
    {
        maxNotes = notesInLevel;
        nextScene = nextSceneName;
    }

    public static int GetRemainingNotes(PlayerInventory inventory)
    {
        Item notes = null;
        inventory.HasItem(ItemProperties.ItemTypes.Note, out notes);

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
