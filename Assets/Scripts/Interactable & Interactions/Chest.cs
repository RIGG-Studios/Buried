using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChestInventory))]
public class Chest : InteractableObject
{
    [SerializeField] private GameObject closedChest;
    [SerializeField] private GameObject openChest;

    [HideInInspector]
    public ChestInventory inventory = null;

    private bool chestShown = false;
    private UIElementGroup inventoryElement;

    private void Awake()
    {
        inventory = GetComponent<ChestInventory>();
    }

    private void Start()
    {
        inventoryElement = CanvasManager.instance.FindElementGroupByID("ChestInventory");
    }

    private void ToggleChest(bool state)
    {
        chestShown = state;
    }

    public override void HoverInteract()
    {
    }

    public override void StopHoverInteract()
    {
    }

    public override void ButtonInteract()
    {
        ToggleChest(true);
        inventory.OnInteract();

        inventoryElement.UpdateElements(0, 0, chestShown);
        GameEvents.OnSearchChest.Invoke();
        open = true;
    }

    public void StopButtonInteract()
    {
        inventory.OnStopInteract();
        ToggleChest(false);
        inventoryElement.UpdateElements(0, 0, false);
        open = false;
    }
}
