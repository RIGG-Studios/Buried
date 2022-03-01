using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    private List<Slot> slots = new List<Slot>();

    public void SetupSlots(int size, Transform slotGrid, ItemDatabase database, GameObject slotPrefab)
    {
        for (int i = 0; i < size; i++)
        {
            Slot slot = Instantiate(slotPrefab, slotGrid).GetComponent<Slot>();

            slot.SetupSlot(database);
            slots.Add(slot);
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
            if(slots[i].item == itemProperties)
            {
                slot = slots[i];
                break;
            }
        }

        return slot;
    }
}

