using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotGrid;
    private List<Slot> slots = new List<Slot>();

    private void Start()
    {
        if (this.slots.Count > 0)
            return;

        Slot[] slots = GetComponentsInChildren<Slot>();

        if(slots.Length > 0)
        {
            for (int i = 0; i < slots.Length; i++)
                this.slots.Add(slots[i]);
        }
    }

    public void SetupSlots()
    {
       Instantiate(slotPrefab, slotGrid).GetComponent<Slot>();
    }

    public Slot GetNextSlot()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].occupied) 
                return slots[i];
        }

        return null;
    }

    public void AddSlot(Item item = null)
    {
        if(FindSlot(item) != null)
        {
            FindSlot(item).UpdateSlotStack(1);
            return;
        }

        Slot slot = GetNextSlot();

        if (slot != null && item != null)
            slot.SetupSlotWithItem(item);
    }

    public Slot FindSlot(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
            if (slots[i].itemIcon == item)
                return slots[i];

        return null;
    }
}
