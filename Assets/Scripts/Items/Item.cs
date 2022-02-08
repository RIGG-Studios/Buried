using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class Item : ScriptableObject
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

/*//
#if UNITY_EDITOR
[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Item item = (Item)target;

        item.itemType = (Item.ItemTypes)EditorGUILayout.EnumPopup("Item Type", item.itemType);
        item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
        item.itemDescription = EditorGUILayout.TextField("Item Description", item.itemDescription);

        item.itemPrefab = EditorGUILayout.ObjectField("Item Prefab", item.itemPrefab, typeof(GameObject)) as GameObject;
        item.itemSprite = EditorGUILayout.ObjectField("Item Sprite", item.itemSprite, typeof(Sprite)) as Sprite;

        item.useInventoryButtons = EditorGUILayout.Toggle("Use Slot Hover Buttons", item.useInventoryButtons);

        if(item.useInventoryButtons)
        {
            item.uiButton = EditorGUILayout.ObjectField("UI Button", item.uiButton, typeof(GameObject)) as GameObject;
            EditorGUILayout.LabelField("Inventory Buttons");
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("uiButtons");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }

        if (item.itemType == Item.ItemTypes.Note)
        {
            item.noteAuthor = EditorGUILayout.TextField("Author", item.noteAuthor);
            item.noteDate = EditorGUILayout.TextField("Date", item.noteDate);
            item.noteDescription = EditorGUILayout.TextField("Contents", item.noteDescription);
        }
        else if(item.itemType == Item.ItemTypes.Consumable)
        {
            item.consumableType = (Item.ConsumableTypes)EditorGUILayout.EnumPopup("Consumable Type", item.consumableType);

            switch (item.consumableType)
            {
                case Item.ConsumableTypes.Battery:
                    item.batteryCapacity = EditorGUILayout.IntField("Battery Capacity (In Seconds)", item.batteryCapacity);
                    item.batteryAmount = EditorGUILayout.IntField("Battery Amount (In Seconds)", item.batteryAmount);
                    break;

                case Item.ConsumableTypes.Healing:
                    item.healAmount = EditorGUILayout.IntField("Heal Amount", item.healAmount);
                    break;
            }

            item.stackable = EditorGUILayout.Toggle("Stackable", item.stackable);

            if (item.stackable)
                item.stackAmount = EditorGUILayout.IntField("Stack Amount", item.stackAmount);
        }
        else if(item.itemType == Item.ItemTypes.Tool)
        {
            item.toolType = (Item.WeaponTypes)EditorGUILayout.EnumPopup("Tool Type", item.toolType);
            item.damage = EditorGUILayout.IntField("Damage", item.damage);
            item.wieldTime = EditorGUILayout.FloatField("Wield Time", item.wieldTime);
            item.unWieldTime = EditorGUILayout.FloatField("Unwield Time", item.unWieldTime);
        }
    }
}

#endif
/*/