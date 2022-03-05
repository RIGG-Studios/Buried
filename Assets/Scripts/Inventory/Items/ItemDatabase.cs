using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SlotManager))]
public class ItemDatabase : MonoBehaviour
{
    public bool initialized { get; private set; }

    protected SlotManager slotManager = null;
    protected List<Item> items = new List<Item>();

    private void Awake()
    {
        slotManager = GetComponent<SlotManager>();
    }

    public bool InitializeDatabase(Item[] startingItems, int inventorySize, Transform slotGrid, GameObject slotPrefab, bool isPlayer)
    {
        slotManager.SetupSlots(inventorySize, slotGrid, slotPrefab, isPlayer);

        if (startingItems.Length > 0)
        {
            for(int i = 0; i < startingItems.Length; i++)
            {
                AddItem(startingItems[i].item, startingItems[i].stack);
            }
        }

        initialized = true;

        return initialized;
    }

    public virtual Item AddItem(ItemProperties itemProperties, int amount)
    {
        if (HasItem(itemProperties))
        {
            Item itm = FindItem(itemProperties);
            UpdateExistingItem(itm, amount);

            return itm; 
        }

        Slot slot = slotManager.GetNextSlot();
        Item item = new Item(itemProperties, slot, amount);

        slot.AddItem(item);
        items.Add(item);
        return item;
    }

    public virtual bool RemoveItem(ItemProperties itemProperties, int amount)
    {
        if (!HasItem(itemProperties))
            return false;

        Item item = FindItem(itemProperties);

        if(item.stack - amount <= 0 || !item.item.stackable)
        {
            slotManager.FindSlotByItemProperties(itemProperties).ResetSlot();
            items.Remove(item);

            return true;
        }

        return UpdateExistingItem(item, -amount);
    }

    private bool UpdateExistingItem(Item item, int amount)
    {
        bool canAddToStack = item.item.stackable;

        if (canAddToStack)
        {
            item.AddToStack(amount);
            slotManager.FindSlotByItemProperties(item.item).UpdateSlotStack(item.stack);
        }

        return canAddToStack;
    }

    public void ResetDatabase()
    {
        slotManager.ResetSlots();
        items.Clear();

        initialized = false;
    }

    public bool HasItem(ItemProperties itemProperties)
    {
        bool hasItem = false;

        for(int i = 0; i < items.Count; i++)
        {
            if(itemProperties == items[i].item)
            {
                hasItem = true;
                break;
            }
        }

        return hasItem;
    }

    public Item FindItem(ItemProperties itemProperties)
    {
        Item item = null;

        for (int i = 0; i < items.Count; i++)
        {
            if (itemProperties == items[i].item)
            {
                item = items[i];
                break;
            }
        }

        return item;
    }

    public Item FindItem(ItemProperties.WeaponTypes weaponType)
    {
        Item item = null;

        for (int i = 0; i < items.Count; i++)
        {
            if (weaponType == items[i].item.toolType)
            {
                item = items[i];
                break;
            }
        }

        return item;
    }

    public Item FindItem(ItemProperties.ConsumableTypes consumableTypes)
    {
        Item item = null;

        for (int i = 0; i < items.Count; i++)
        {
            if (consumableTypes == items[i].item.consumableType)
            {
                item = items[i];
                break;
            }
        }

        return item;
    }

    public Item[] FindAllTools()
    {
        List<Item> items = new List<Item>();

        for (int i = 0; i < this.items.Count; i++)
        {
            if (this.items[i].item.toolType != ItemProperties.WeaponTypes.None)
            {
                items.Add(this.items[i]);
                break;
            }
        }

        return items.ToArray();
    }

    public Item[] FindAllConsumables()
    {
        List<Item> items = new List<Item>();

        for (int i = 0; i < this.items.Count; i++)
        {
            if (this.items[i].item.consumableType != ItemProperties.ConsumableTypes.None)
            {
                items.Add(this.items[i]);
                break;
            }
        }

        return items.ToArray();
    }

    public bool TryEnableSlot(int index, out Item item)
    {
       return slotManager.TryEnableSlot(index, out item);
    }

    public Item[] GetItems()
    {
        return items.ToArray();
    }
}

[System.Serializable]
public class Item
{
    public ItemProperties item;
    public int stack = 1;
    public Slot slot { get; set; }

    public Item(ItemProperties item, Slot slot, int stack)
    {
        this.item = item;
        this.slot = slot;
        this.stack = stack;
    }

    public void AddToStack(int amount)
    {
        stack += amount;
        stack = Mathf.Clamp(stack, 0, item.stackAmount);
    }
}
