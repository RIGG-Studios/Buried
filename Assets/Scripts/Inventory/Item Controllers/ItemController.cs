using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base class for item controllers, since we need a way for items to have functionaility (like a flashlight or a grappling hook)
//they have some variables/methods that relate. This class handles all the base functionality of those items
public class ItemController : MonoBehaviour
{
    //when this items spawns in, where?
    public Vector3 startingPosition;
    //when this item spawns in, what rotation
    public Vector3 startingRotation;
    //item this controller relates too
    public ItemProperties baseItem;

    public Item itemInInventory { get; private set; }

    //player
    protected Player player;

    //setup controller, used to assign the player
    public virtual void SetupController(Player player, Item itemInInventory)
    {
        this.player = player;
        this.itemInInventory = itemInInventory;
    }

    //use item, if this item is a flashlight toggle it on or off, or if this is a grappling hook try and fire the grappling hook.
    public virtual void UseItem() { }
    //resets the item, if we are using another item there is no need to keep using the old item. This method disabled items that arent used.
    public virtual void ResetItem() { }
}
