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

    public void InitializeDatabase(Item[] startingItems, int inventorySize, Transform slotGrid, GameObject slotPrefab, bool isPlayer)
    {
        slotManager.SetupSlots(inventorySize, slotGrid, this, slotPrefab, isPlayer);

        if (startingItems.Length > 0)
        {
            for(int i = 0; i < startingItems.Length; i++)
            {
                AddItem(startingItems[i].item, startingItems[i].stack);
            }
        }

        initialized = true;
    }

    public virtual bool AddItem(ItemProperties itemProperties, int amount)
    {
        if (HasItem(itemProperties))
        {
            return UpdateExistingItem(FindItem(itemProperties), amount);
        }

        Item item = new Item(itemProperties, amount);

        slotManager.GetNextSlot().AddItem(item, amount);
        items.Add(item);
        return true;
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
}

[System.Serializable]
public class Item
{
    public ItemProperties item;
    public int stack;

    public Item(ItemProperties item, int stack)
    {
        this.item = item;
        this.stack = stack;
    }

    public void AddToStack(int amount)
    {
        stack += amount;
        stack = Mathf.Clamp(stack, 0, item.stackAmount);
    }
}
