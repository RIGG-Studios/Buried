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
    private UIElement noteContents = null;
    private ButtonElements nextButton = null;
    private ButtonElements previousButton = null;

    private void Awake()
    {
        noteGroup = CanvasManager.instance.FindElementGroupByID("Journal");

        if(noteGroup != null)
        {
            noteHeader = noteGroup.FindElement("header");
            noteContents = noteGroup.FindElement("contents");
            nextButton = (ButtonElements)noteGroup.FindElement("nextbutton");
            previousButton = (ButtonElements)noteGroup.FindElement("previousbutton");
        }
    }

    private void Start()
    {
    //    player.playerInput.Player.Leave.performed += ctx => ResetItem();
    }

    public override void SetupController(Player player, Item itemInInventory)
    {
        base.SetupController(player, itemInInventory);

        nextButton.button.onClick.AddListener(() => CyclePages(1));
        previousButton.button.onClick.AddListener(() => CyclePages(-1));
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

    private void CyclePages(int amount)
    {
        if (notesInJournal.Count <= 0)
            return;

        currentPageIndex += amount;

        if (currentPageIndex > notesInJournal.Count - 1)
            currentPageIndex = 0;
        else if (currentPageIndex < 0)
            currentPageIndex = notesInJournal.Count - 1;

        ItemProperties note = notesInJournal[currentPageIndex];

        if(note != null)
        {
            noteContents.SetActive(true);
            noteHeader.OverrideValue("Note " + (currentPageIndex + 1));
            noteContents.OverrideValue(note.noteSprite);
        }
    }

    public void AddNote(ItemProperties note)
    {
        if (notesInJournal.Contains(note))
            return;

        notesInJournal.Add(note);
    }

    private void LoadNotes()
    {
        CyclePages(0);
    }

    public override void ResetItem()
    {
        if (state == JournalStates.Open)
        {
            UseItem();
            player.inventory.DeselectSlot(itemInInventory);
        }
    }
}
