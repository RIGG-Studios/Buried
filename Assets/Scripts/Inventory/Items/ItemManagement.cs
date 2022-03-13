using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This class is the manager of items that need a controller. Since it is not fundemental to the database
//and is only used for the player, this class is seperate from the inventory.
public class ItemManagement : MonoBehaviour
{
    private Player player { get { return GetComponent<Player>(); } }

    [SerializeField] private Transform itemParent;

    public ItemController activeController { get; private set; } 
    private List<ItemController> allItems = new List<ItemController>();

    private UIElementGroup equipGroup;
    private SliderElement equipTimer;
    private UIElement equipText;

    private bool canUseItems;

    private void Start()
    {
        equipGroup = CanvasManager.instance.FindElementGroupByID("EquipGroup");

        if(equipGroup != null)
        {
            equipTimer = (SliderElement)equipGroup.FindElement("flashlightslider");
            equipText = equipGroup.FindElement("text");
        }

        player.playerInput.Player.Fire.performed += ctx => UseActiveController();
        player.playerInput.Enable();
    }

    private void UseActiveController()
    {
        if (activeController == null || !canUseItems)
            return;

        activeController.UseItem();
    }

    public void InitializeItemControllers(Item[] itemsToSpawn)
    {
        if (itemsToSpawn.Length <= 0)
            return;

        for(int i = 0; i < itemsToSpawn.Length; i++)
        {
            OnNewItemAdded(itemsToSpawn[i]);
        }
    }

    public void SetNewActiveController(ItemProperties item)
    {
        if (activeController != null && item == activeController.baseItem)
            return;

        equipGroup.UpdateElements(0, 0, true);
        equipTimer.SetMax(1.5f);
        equipTimer.SetMin(0);
        canUseItems = false;
        StartCoroutine(EquipNewItem(item));
    }

    private IEnumerator EquipNewItem(ItemProperties nextItem)
    {
        float elapsedTime = 1.5f;

        while (elapsedTime >= 0)
        {
            equipTimer.OverrideValue(elapsedTime);
            equipText.OverrideValue("Equipping " + nextItem.itemName +  " in: " + (int)elapsedTime);
            elapsedTime -= Time.deltaTime * nextItem.equipTime;
            yield return null;
        }

        if (activeController)
        {
            activeController.itemInInventory.slot.SetColor(activeController.itemInInventory.slot.dColor);
            activeController.ResetItem();
        }
        //set the color of the slot to the highlighted slot, because this slot is now activiated.
        activeController = FindItemController(nextItem);
        activeController.itemInInventory.slot.SetColor(activeController.itemInInventory.slot.hColor);
        canUseItems = true;
        equipGroup.UpdateElements(0, 0, false);
    }

    public void OnNewItemAdded(Item item)
    {
        ItemController newItem = Instantiate(item.item.itemPrefab, itemParent).GetComponent<ItemController>();

        if(newItem != null)
        {
            newItem.transform.localPosition = newItem.startingPosition;
            newItem.transform.localRotation = Quaternion.Euler(newItem.startingRotation);

            newItem.SetupController(player, item);
            allItems.Add(newItem);
        }
    }

    public bool OnItemRemoved(ItemProperties item)
    {
        ItemController itemToRemove = FindItemController(item);

        if(itemToRemove != null)
        {
            allItems.Remove(itemToRemove);
            Destroy(itemToRemove.gameObject);

            return true;
        }

        return false;
    }

    public ItemController FindItemController(ItemProperties item)
    {
        for(int i = 0; i < allItems.Count; i++)
        {
            if (item == allItems[i].baseItem)
                return allItems[i];
        }

        return null;
    }
}
