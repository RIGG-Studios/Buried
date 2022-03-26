using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class ItemProperties : ScriptableObject
{
    public enum ItemTypes
    {
        Battery,
        Flashlight,
        GrapplingHook,
        Journal,
        Note,
        GrapplingHookAmmo,
        Flare,
        None
    }

    public enum ActivationTypes
    {
        OnSlotSelected,
        RightMouseClick,
        None
    }

    public string itemName;
    public string itemDescription;
    public ItemTypes itemType;
    public ActivationTypes activateType;

    public Sprite itemSprite;
    public GameObject itemPrefab;

    public bool controllable;
    public float equipTime;

    public bool stackable;
    public int stackAmount;

    public GameObject button;
    public bool inventoryButtons;
    public UIButtonProperties[] uiInventoryButtons;
    public bool chestButtons;
    public UIButtonProperties[] uiChestButtons;

    //note
    public string author;
    public string date;
    public Sprite noteSprite;


    /*/
    public enum ItemTypes
    {
        None,
        Note,
        Tool,
        Consumable,
    }

    public enum ConsumableTypes
    {
        None,
        Battery,
    }

    public enum WeaponTypes
    {
        None,
        Pickaxe,
        Flashlight,
        GrapplingHook
    }

    public string itemName;
    public string itemDescription;
    public ItemTypes itemType;

    public Sprite itemSprite;
    public GameObject itemPrefab;

    public bool stackable;
    public int stackAmount;

    //note properties
    public Sprite note = null;

    //weapon properties
    public WeaponTypes toolType;
    public int damage;
    public float wieldTime;
    public float unWieldTime;

    //consumable properties
    public ConsumableTypes consumableType;
    public int batteryCapacity;
    public int batteryAmount;
    public int healAmount;

    public bool useUIButtons;
    public GameObject uiButton;
    public UIButtonProperties[] uiInventoryButtons;
    public UIButtonProperties[] uiChestButtons;
    /*/
}

[System.Serializable]
public class UIButtonProperties
{
    public enum PropertyTypes
    {
        ShowProperty,
        Use,
        Discard,
        AddToInventory
    }


    public string name;
    public PropertyTypes propertyType;
    [Tooltip("Only use if the property type is ShowProperty")] public string propertyName;
}
