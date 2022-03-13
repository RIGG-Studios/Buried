using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JournalStates
{
    Open,
    Closed
}

public class JournalController : ItemController
{
    [SerializeField] private JournalStates state = JournalStates.Closed;
    [SerializeField, Range(0, 10)] private int maxNotesInJournal = 5;

    public List<ItemProperties> notesInJournal = new List<ItemProperties>();
    private int currentPageIndex = 0;


    private UIElementGroup noteGroup = null;
    private UIElement noteHeader = null;
    private UIElement noteDate = null;
    private UIElement noteAuthor = null;
    private UIElement noteDescription = null;
    private ButtonElements nextButton = null;
    private ButtonElements previousButton = null;

    private void Awake()
    {
        noteGroup = CanvasManager.instance.FindElementGroupByID("Journal");

        if(noteGroup != null)
        {
            noteHeader = noteGroup.FindElement("header");
            noteDate = noteGroup.FindElement("date");
            noteAuthor = noteGroup.FindElement("author");
            noteDescription = noteGroup.FindElement("contents");

            nextButton = (ButtonElements)noteGroup.FindElement("nextbutton");
            previousButton = (ButtonElements)noteGroup.FindElement("previousbutton");
        }
    }

    private void Start()
    {
        player.playerInput.Player.Leave.performed += ctx => DisableJournal();
    }

    public override void SetupController(Player player, Item itemInInventory)
    {
        base.SetupController(player, itemInInventory);

        nextButton.button.onClick.AddListener(() => CyclePages(currentPageIndex++));
        previousButton.button.onClick.AddListener(() => CyclePages(currentPageIndex--));

    }

    public override void UseItem()
    {
        if(state == JournalStates.Closed)
        {
            LoadNotes();
            noteGroup.UpdateElements(0, 0, true);
            state = JournalStates.Open;
        }
        else if(state == JournalStates.Open)
        {
            noteGroup.UpdateElements(0, 0, false);
            state = JournalStates.Closed;
        }
    }

    private void CyclePages(int index)
    {
        if(index > notesInJournal.Count)
        {
            index = 0;
        }
        else if(index < 0)
        {
            index = notesInJournal.Count - 1;
        }

        ItemProperties note = notesInJournal[index];

        if(note != null)
        {
            noteHeader.OverrideValue("NOTE HEADER");
       //     noteDate.OverrideValue(note.);
        }

        currentPageIndex = index;
    }

    private void LoadNotes()
    {
        CyclePages(0);
    }

    public void AddNewNote(string header, string author, string date, string contents)
    {
        /*/
        Note newNote = new Note(author, date, header, contents);

        if(notesInJournal.Contains(newNote) || notesInJournal.Count < maxNotesInJournal)
        {
            notesInJournal.Add(newNote);
        }
        /*/
    }

    private void DisableJournal()
    {
        if (state == JournalStates.Open)
            UseItem();
    }
}
