using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class ItemProperties : ScriptableObject
{
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
        Healing
    }

    public enum WeaponTypes
    {
        None,
        Pickaxe,
        Flashlight
    }

    public string itemName;
    public string itemDescription;
    public ItemTypes itemType;

    public Sprite itemSprite;
    public GameObject itemPrefab;

    public bool stackable;
    public int stackAmount;

    //note properties
    public string noteAuthor;
    public string noteDate;
    [TextArea]
    public string noteDescription;

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

    public bool useInventoryButtons;
    public GameObject uiButton;
    public UIButtonProperties[] uiButtons;
}

[System.Serializable]
public class UIButtonProperties
{
    public enum PropertyType
    {
        ShowProperty,
        Use,
        Discard
    }

    public string name;
    public PropertyType propertyType;
    [Tooltip("Only use if the property type is ShowProperty")] public string propertyName;
}
