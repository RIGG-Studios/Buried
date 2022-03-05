using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemManagement : MonoBehaviour
{
    [SerializeField] private Transform itemParent = null;

    private List<ItemController> itemControllers = new List<ItemController>();
    private ItemController activeItemController = null;

    private Player player = null;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        player.playerInput.Player.Fire.performed += ctx => UseActiveItem();
    }

    private void UseActiveItem()
    {
        if (activeItemController == null)
            return;

        activeItemController.UseItem();
    }

    public void SetNewItemController(ItemProperties props)
    {
        ItemController nextController = FindItemController(props);

        if (nextController && activeItemController != nextController)
        {
            if (activeItemController)
            {
                activeItemController.ResetItem();
                activeItemController.baseItem.slot.SetColor(activeItemController.baseItem.slot.dColor);
            }

            activeItemController = nextController;
        }
    }

    public void UseItem(Item item)
    {
        if (item == null)
            return;

        ItemController controller = FindItemController(item);

        if (controller)
            controller.UseItem();
    }

    public void SetupItemControllers(Item[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            SpawnItemController(items[i]);
        }
    }
    private void SpawnItemController(Item item)
    {
        ItemController itm = Instantiate(item.item.itemPrefab, itemParent).GetComponent<ItemController>();
        itm.transform.localPosition = itm.startingPosition;
        itm.transform.localRotation = Quaternion.Euler(itm.startingRotation);
        itm.baseItem = item;
        itm.SetupController(player);
        itemControllers.Add(itm);
    }

    public void SetupNewItem(Item item)
    {
        SpawnItemController(item);
    }

    public void RemoveItem(ItemProperties item)
    {
        ItemController controller = FindItemController(item);

        if(controller != null)
        {
            itemControllers.Remove(controller);
            Destroy(controller);
        }
    }

    public ItemController FindItemController(Item item)
    {
        for(int i = 0; i < itemControllers.Count; i++)
        {
            if (item == itemControllers[i].baseItem)
                return itemControllers[i];
        }

        return null;
    }

    public ItemController FindItemController(ItemProperties item)
    {
        for (int i = 0; i < itemControllers.Count; i++)
        {
            if (item == itemControllers[i].baseItem.item)
                return itemControllers[i];
        }

        return null;
    }

    public ItemController GetActiveController()
    {
        return activeItemController;
    }
}
