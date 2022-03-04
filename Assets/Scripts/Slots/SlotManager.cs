using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    private List<Slot> slots = new List<Slot>();

    public void SetupSlots(int size, Transform slotGrid, GameObject slotPrefab, bool isPlayer)
    {
        for (int i = 0; i < size; i++)
        {
            Slot slot = Instantiate(slotPrefab, slotGrid).GetComponent<Slot>();

            slot.SetupSlot(this, isPlayer, i);
            slots.Add(slot);
        }
    }

    public void TryEnableSlot(int index)
    {
        if (index > slots.Count - 1 || index < 0)
            return;

        slots[index].UseItem();
    }

    public void SwitchItemsInSlots(Slot baseSlot, Slot nextSlot)
    {
        if (baseSlot == nextSlot)
            return;

        if (nextSlot.hasItem)
        {
            Item baseItem = baseSlot.item;
            Item nextItem = nextSlot.item;

            baseSlot.ResetSlot();
            baseSlot.AddItem(nextItem);

            nextSlot.ResetSlot();
            nextSlot.AddItem(baseItem);
        }
        else
        {
            nextSlot.AddItem(baseSlot.item);
            baseSlot.ResetSlot();
        }
    }

    public Slot GetNextSlot()
    {
        Slot slot = null;

        for(int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].hasItem)
            {
                slot = slots[i];
                break;
            }
        }

        return slot;
    }

    public Slot FindSlotByItemProperties(ItemProperties itemProperties)
    {
        Slot slot = null;
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].hasItem)
                continue;

            if(slots[i].item.item == itemProperties)
            {
                slot = slots[i];
                break;
            }
        }

        return slot;
    }

    public void ResetSlots()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            Destroy(slots[i].gameObject);
        }

        if (slots.Count > 0)
            slots.Clear();
    }

    public Slot[] GetAllSlots()
    {
        return slots.ToArray();
    }

    public Slot[] GetAllSlotsInScene()
    {
        return FindObjectsOfType<Slot>();
    }
}

