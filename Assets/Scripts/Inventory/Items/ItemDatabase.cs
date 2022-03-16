using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this class is a base inventory manager, any inventory system can use this as a base and will get a fully working system. That can add/remove different items
//with stacks, etc.
[RequireComponent(typeof(SlotManager))]
public class ItemDatabase : MonoBehaviour
{
    //lets us know if the database has been properly intialized
    public bool initialized { get; private set; }

    //database needs a slot manager as the inventory is slot based
    protected SlotManager slotManager = null;
    
    //list of items in the database
    protected List<Item> items = new List<Item>();
    private int inventoySize;

    private void Awake()
    {
        slotManager = GetComponent<SlotManager>();
    }

    //Method used to initialize the database, taking in any potential starting items, the size, slot info and if this is the player inventory
    public bool InitializeDatabase(Item[] startingItems, int inventorySize, Transform slotGrid, GameObject slotPrefab, bool isPlayer)
    {
        //setup the slots with the given arguments
        slotManager.SetupSlots(inventorySize, slotGrid, slotPrefab, isPlayer);

        inventoySize = inventorySize;

        //if we have given any starting items, like chest loot or starting items for the player, follow through
        if (startingItems.Length > 0)
        {
            //loop through all the starting items
            for(int i = 0; i < startingItems.Length; i++)
            {
                if (isPlayer && startingItems[i].item.controllable)
                    continue;

                //add them to the inventory
                AddItem(startingItems[i].item, startingItems[i].stack);
            }
        }


        //database is now setup
        initialized = true;

        //return the final result
        return initialized;
    }

    //method used for adding in items, taking in the item properties and the amount, and returning an new Item that the inventory uses.
    public virtual Item AddItem(ItemProperties itemProperties, int amount)
    {
        if (items.Count >= inventoySize)
            return null;

        //check if we already have this item in the inventory
        if (HasItem(itemProperties))
        {
            //if so, find the item variable
            Item itm = FindItem(itemProperties);
            //update that item with the given amount
            UpdateExistingItem(itm, amount);
            //since we already have this item and we called the UpdateExistingItem to try update its stack, return the item already in the database
            return itm; 
        }

        //Find the next avaliable slot in the SlotManager
        Slot slot = slotManager.GetNextSlot();
        //create a new item with the item props, slot, and stack amount
        Item item = new Item(itemProperties, slot, amount);

        //add the item info to the slot
        slot.AddItem(item);
        //add the item to the items
        items.Add(item);
        //finally return the newly made item
        return item;
    }

    //method used for removing items
    public virtual bool RemoveItem(ItemProperties itemProperties, int amount)
    {
        //if we do not have this item in the database, stop this method because there is no item to remove.
        if (!HasItem(itemProperties))
            return false;

        //since we are removing an item, we can call the FindItem method
        Item item = FindItem(itemProperties);

        //if our next stack will be less then 0 or this item is not stackable, remove the item from the slot
        if(item.stack - amount <= 0 || !item.item.stackable)
        {
            //reset the slot as this is no longer in our inventory
            item.slot.ResetSlot();
            //remove the item from the database
            items.Remove(item);

            //succesfully removed the item
            return true;
        }

        //if we have a stack, call the UpdateExistingItem method to update its stack.
        return UpdateExistingItem(item, -amount);
    }

    //method used for updating item with stacks in the database, since not all items have a stack this is only called for items that can stack (batteries for example).
    private bool UpdateExistingItem(Item item, int amount)
    {
        //check if this items stack can be updated
        bool canAddToStack = item.item.stackable;

        if (canAddToStack)
        {
            //if we can update the stack, update the item stack
            item.AddToStack(amount);
            //update the slot ui stack
            item.slot.UpdateSlotStack(item.stack);
        }

        return canAddToStack;
    }

    //method used for resetting a database to nothing, with no items or anything assigned to it.
    public void ResetDatabase()
    {
        //reset the slots
        slotManager.ResetSlots();
        //clear the items
        items.Clear();

        //we are no longer setup
        initialized = false;
    }

    //method for checking if we have an item in the database, returning a bool
    public bool HasItem(ItemProperties itemProperties)
    {
        bool hasItem = false;

        //loop through all items
        for(int i = 0; i < items.Count; i++)
        {
            //if any in the list contain the argument, return it and break.
            if(itemProperties == items[i].item)
            {
                hasItem = true;
                break;
            }
        }

        return hasItem;
    }

    //method for checking if we have an item in the database, returning a bool
    public bool HasItem(ItemProperties.ItemTypes itemType)
    {
        bool hasItem = false;

        //loop through all items
        for (int i = 0; i < items.Count; i++)
        {
            //if any in the list contain the argument, return it and break.
            if (itemType == items[i].item.itemType)
            {
                hasItem = true;
                break;
            }
        }

        return hasItem;
    }

    //method used for finding an item in the database
    public Item FindItem(ItemProperties itemProperties)
    {
        Item item = null;

        //loop through all the items
        for (int i = 0; i < items.Count; i++)
        {
            //check if any contain the argument given
            if (itemProperties == items[i].item)
            {
                item = items[i];
                break;
            }
        }

        return item;
    }

    public Item FindItem(ItemProperties.ItemTypes itemType)
    {
        Item item = null;

        for (int i = 0; i < items.Count; i++)
        {
            if (itemType == items[i].item.itemType)
            {
                item = items[i];
                break;
            }
        }

        return item;
    }

    public void TryFindItem(ItemProperties.ItemTypes type, out Item item)
    {
        item = null;

        for (int i = 0; i < items.Count; i++)
        {
            if (type == items[i].item.itemType)
                item = items[i];         
        }
    }

    public Item[] FindAllControllableItems()
    {
        List<Item> items = new List<Item>();

        for (int i = 0; i < this.items.Count; i++)
        {
            if (this.items[i].item.controllable)
            {
                items.Add(this.items[i]);
                break;
            }
        }

        return items.ToArray();
    }


    //method used for trying to enable a slot, since slots have items in it we want to be able to interact with those items. So we every time we press a button that relates
    //to a certain slot, like (1,2,3,4,5,6) find the slot the relates and check if it has an item that can be activated like a flashlight, or hookshot. 
    //If we found the item in the slot, we will provide it using the out and enable the item.
    public bool TryEnableSlot(int index, out Item item)
    {
       return slotManager.TryEnableSlot(index, out item);
    }

    //method used for quickly grabbing all items in the database to outside classes
    public Item[] GetItems()
    {
        return items.ToArray();
    }

}

//This class is used for local items, so we can track its stack, slot, and item properties.
[System.Serializable]
public class Item
{
    //item properties this belongs too
    public ItemProperties item;
    
    //the stack of the item, non-stackable items will default to 1
    public int stack = 1;

    //property for the slot this item is in
    public Slot slot { get; set; }

    //constructor to assign all the values
    public Item(ItemProperties item, Slot slot, int stack)
    {
        this.item = item;
        this.slot = slot;
        this.stack = stack;
    }
    
    //method to add to stack
    public void AddToStack(int amount)
    {
        stack += amount;
        stack = Mathf.Clamp(stack, 0, item.stackAmount);
    }
}
