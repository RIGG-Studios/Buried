using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotGrid;


    List<Slot> slots = new List<Slot>();
    Inventory inventory;

    public void SetupSlots(int size, Inventory inventory)
    {
        for (int i = 0; i < size; i++)
        {
            Slot slot = Instantiate(slotPrefab, slotGrid).GetComponent<Slot>();

            slots.Add(slot);
        }

        this.inventory = inventory;
    }

    public void AddSlot(bool inventoryButtons, Item item)
    {
        if(FindSlot(item) != null && item.stackable)
        {
            Slot s = FindSlot(item);
            s.UpdateSlotStack(1);
            return;
        }

        Slot slot = GetNextSlot();

        if (slot != null && item != null)
            slot.SetupSlotWithItem(inventory, item, inventoryButtons);
    }

    public void RemoveSlot(Item item)
    {
        if (FindSlot(item) != null && item.stackable)
        {
            Slot s = FindSlot(item);
            s.UpdateSlotStack(-1);

            if(FindSlot(item).GetStack() >= 1)
                return;
        }

        Slot slot = FindSlot(item);

        if (slot != null)
            slot.ResetSlot();
    }

    public Slot FindSlot(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
            if (slots[i].item == item)
                return slots[i];

        return null;
    }

    public Slot GetNextSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].occupied)
                return slots[i];
        }

        return null;
    }
}
