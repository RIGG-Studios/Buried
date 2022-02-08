using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    [System.Serializable]
    public class ChestItemProperties
    {
        public Item item;
        public int stack;
    }

    public GameObject closedChest;
    public GameObject openChest;

    public List<ChestItemProperties> itemsInChest = new List<ChestItemProperties>();

    bool chestShown;
    Player player;
    SlotManager slotManager;

    private void Start()
    {
        slotManager = GetComponent<SlotManager>();
    }

    public override void Interact(Player player)
    {
        this.player = player;
        ToggleChest();
    }

    private void ToggleChest()
    {
        if (chestShown)
        {
            openChest.SetActive(false);
            closedChest.SetActive(true);
            chestShown = false;
        }
        else
        {
            openChest.SetActive(true);
            closedChest.SetActive(false);
            chestShown = true;

            ShowChestLoot();
        }
    }

    public override void StopInteract()
    {
    }

    private void ShowChestLoot()
    {
        slotManager.SetupSlots(itemsInChest.Count, player.inventory);

        for(int i = 0; i < itemsInChest.Count; i++)
        {
            slotManager.AddSlot(false, itemsInChest[i].item);
        }
    }
}
