using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public SlotManager slotManager;
    public Item flashlight;
    public Item flashlightBattery;
    public Player player;
    public PlayerUI playerUI;

    private void Start()
    {
        slotManager.SetupSlots(4, this);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        slotManager.AddSlot(true, item);
    }

    private void RemoveItem(Item item)
    {
        if (!items.Contains(item))
            return;

        items.Remove(item);
        slotManager.RemoveSlot(item);
    }

    public void ShowItemProperty(Item item, string property)
    {     
        if(property.Contains("Battery"))
        {
            playerUI.ShowFlashlightBattery(player.flashLightManager.GetCurrentLightIntensity(),
                player.flashLightManager.GetCurrentMaxLightIntensity());
        }
    }

    public void UseItem(Item item)
    {
        if(item.itemType == Item.ItemTypes.Tool)
        {
            switch (item.toolType)
            {
                case Item.WeaponTypes.Flashlight:
                    player.DoAction(PlayerActions.ToggleFlashlight);
                    break;

                case Item.WeaponTypes.Pickaxe:

                    break;
            }
        }
        else if(item.itemType == Item.ItemTypes.Consumable)
        {
            switch (item.consumableType)
            {
                case Item.ConsumableTypes.Battery:
                    if (HasItem(Item.WeaponTypes.Flashlight))
                    {
                        int batteryCapacity = item.batteryCapacity;
                        int battery = item.batteryAmount;

                        int batteryLife = batteryCapacity - battery;

                        player.flashLightManager.UpdateBatteryLife(batteryLife);
                        RemoveItem(item);
                    }
                    break;
            }
        }
    }

    public void DiscardItem(Item item)
    {

    }

    public bool HasItem(Item.WeaponTypes toolType)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (toolType == items[i].toolType)
                return true;
        }

        return false;
    }
    public bool HasItem(Item.ItemTypes itemType)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (itemType == items[i].itemType)
                return true;
        }

        return false;
    }
    public bool HasItem(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (name == items[i].itemName)
                return true;
        }

        return false;
    }
}
