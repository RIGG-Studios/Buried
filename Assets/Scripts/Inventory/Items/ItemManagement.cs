using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This class is the manager of items that need a controller. Since it is not fundemental to the database
//and is only used for the player, this class is seperate from the inventory.
public class ItemManagement : MonoBehaviour
{
    //parent of spawned item controllers
    [SerializeField] private Transform itemParent = null;

    //list of all items controllers we have in the list
    private List<ItemController> itemControllers = new List<ItemController>();
    //item controller we are currently using
    private ItemController activeItemController = null;
    //reference to the player
    private Player player = null;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        //when we press any key that is under the Fire action, try and use the active item.
        player.playerInput.Player.Fire.performed += ctx => UseActiveItem();
    }

    public void UseActiveItem()
    {
        //if we have no active item, dont use it.
        if (activeItemController == null)
            return;

        //use the current item controller.
        activeItemController.UseItem();
    }

    //method used for setting a new item controller, when we enable another item controller in the inventory, this method is called
    //to reassign it to the next highlighted item.
    public void SetNewItemController(ItemProperties props)
    {
        //find the controller with the given properties
        ItemController nextController = FindItemController(props);

        //if we have found the next controller, and this controller is not the next controller
        if (nextController && activeItemController != nextController)
        {
            //check if we already have an active item controller, so we can disable it.
            if (activeItemController)
            {
                //reset the item
                activeItemController.ResetItem();
                //set the color of the slot back to the default color.
                activeItemController.baseItem.slot.SetColor(activeItemController.baseItem.slot.dColor);
            }

            //finally, assign the current controller to the next one
            activeItemController = nextController;
        }
    }

    //Method used for setting up items with any starting items the player may have
    public void SetupItemControllers(Item[] items)
    {
        //loop through all the items
        for(int i = 0; i < items.Length; i++)
        {
            //spawn them
            SpawnItemController(items[i]);
        }
    }
    private void SpawnItemController(Item item)
    {
        //create a new item controller
        ItemController itm = Instantiate(item.item.itemPrefab, itemParent).GetComponent<ItemController>();
        //set its position to the starting pos
        itm.transform.localPosition = itm.startingPosition;
        //set the rotation to the starting rot
        itm.transform.localRotation = Quaternion.Euler(itm.startingRotation);
        //set the base item to the next item
        itm.baseItem = item;
        //setup the controller
        itm.SetupController(player);
        //add it the the list
        itemControllers.Add(itm);
    }

    //Method used to setup individual items or items added to the inventory via a chest.
    public void SetupNewItem(Item item)
    {
        //spawn in a new item controller with the given item.
        SpawnItemController(item);
    }

    //method used for removing item controllers. If the player ever loses an item, remove its controller aswell.
    public void RemoveItem(ItemProperties item)
    {
        //find the controller
        ItemController controller = FindItemController(item);

        if(controller != null)
        {
            //remove & destroy it
            itemControllers.Remove(controller);
            Destroy(controller);
        }
    }


    //method used for finding item controllers
    public ItemController FindItemController(Item item)
    {
        //loop through all items
        for(int i = 0; i < itemControllers.Count; i++)
        {
            //check if we found any that is the same as the item
            if (item == itemControllers[i].baseItem)
                return itemControllers[i];
        }

        return null;
    }


    //same method as above, but use the ItemProperties instead
    public ItemController FindItemController(ItemProperties item)
    {
        for (int i = 0; i < itemControllers.Count; i++)
        {
            if (item == itemControllers[i].baseItem.item)
                return itemControllers[i];
        }

        return null;
    }

    //method used for finding the active controller.
    public ItemController GetActiveController()
    {
        return activeItemController;
    }
}
