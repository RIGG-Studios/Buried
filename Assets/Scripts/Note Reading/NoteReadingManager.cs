using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteReadingManager : MonoBehaviour
{
    public static NoteReadingManager instance;

    private UIElementGroup noteGroup = null;
    private UIElement noteSprite = null;
    private Player player = null;
    private ItemProperties currentNote = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        noteGroup = CanvasManager.instance.FindElementGroupByID("Note");

        if (noteGroup != null)
        {
            noteSprite = noteGroup.FindElement("notesprite");
        }
    }

    public void ReadNote(ItemProperties note)
    {
        if (note == null)
            return;

        currentNote = note;
        noteSprite.OverrideValue(currentNote.noteSprite);
        noteGroup.UpdateElements(0f, 0f, true);
    }

    public void HideNote()
    {
        noteGroup.UpdateElements(0f, 0f, false);
        player.inventory.AddItem(currentNote, 1);
    }
}
