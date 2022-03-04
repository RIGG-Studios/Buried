using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemManagement : MonoBehaviour
{
    [SerializeField] private Transform itemParent = null;

    private List<ItemController> itemControllers = new List<ItemController>();
    private Player player = null;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        player.playerInput.Player.Flashlight.performed += ctx => ToggleItem(player.inventory.FindItem(ItemProperties.WeaponTypes.Flashlight));
    }


    public void SetupItemControllers(ItemProperties[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            SpawnItemController(items[i]);
        }
    }

    private void SpawnItemController(ItemProperties properties)
    {
        ItemController item = Instantiate(properties.itemPrefab, itemParent).GetComponent<ItemController>();
        item.transform.localPosition = item.startingPosition;
        item.transform.localRotation = Quaternion.Euler(item.startingRotation);
        item.baseItem = properties;
        item.SetupController(player);
        itemControllers.Add(item);
    }

    public void SetupNewItem(ItemProperties item)
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

    public void ToggleItem(Item item)
    {
        if (item == null)
            return;

        ItemController controller = FindItemController(item);

        if (controller)
            controller.UseItem();
    }


    public ItemController FindItemController(Item item)
    {
        for(int i = 0; i < itemControllers.Count; i++)
        {
            if (item.item == itemControllers[i].baseItem)
                return itemControllers[i];
        }

        return null;
    }

    public ItemController FindItemController(ItemProperties item)
    {
        for (int i = 0; i < itemControllers.Count; i++)
        {
            if (item == itemControllers[i].baseItem)
                return itemControllers[i];
        }

        return null;
    }
}
