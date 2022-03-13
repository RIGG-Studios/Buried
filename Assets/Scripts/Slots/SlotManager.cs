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

    public bool TryEnableSlot(int index, out Item item)
    {
        bool success = false;
        item = null;
        if (index > slots.Count - 1 || index < 0)
        {
            item = null;
            success = false;
        }

        if (slots[index].hasItem)
        {
            slots[index].UseItem();
            item = slots[index].item;
            success = true;
        }

        return success;
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

    public void SetSlotTo(Slot startingSlot, Vector3 position)
    {
        float closestDist = -1f;
        Slot closestSlot = null;

        for (int i = 0; i < slots.Count; i++)
        {
            float dist = (position - slots[i].transform.position).magnitude;

            if (closestSlot == null || dist < closestDist)
            {
                closestSlot = slots[i];
                closestDist = dist;
            }
        }

        if (closestSlot != null && closestDist <= 25f)
        {
            SwitchItemsInSlots(startingSlot, closestSlot);
        }
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

