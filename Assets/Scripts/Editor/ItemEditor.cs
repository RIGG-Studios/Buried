using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ItemProperties))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemProperties item = (ItemProperties)target;

        item.itemType = (ItemProperties.ItemTypes)EditorGUILayout.EnumPopup("Item Type", item.itemType);
        item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
        item.itemDescription = EditorGUILayout.TextField("Item Description", item.itemDescription);

        item.itemPrefab = EditorGUILayout.ObjectField("Item Prefab", item.itemPrefab, typeof(GameObject)) as GameObject;
        item.itemSprite = EditorGUILayout.ObjectField("Item Sprite", item.itemSprite, typeof(Sprite)) as Sprite;

        item.useInventoryButtons = EditorGUILayout.Toggle("Use Slot Hover Buttons", item.useInventoryButtons);

        if (item.useInventoryButtons)
        {
            item.uiButton = EditorGUILayout.ObjectField("UI Button", item.uiButton, typeof(GameObject)) as GameObject;
            EditorGUILayout.LabelField("Inventory Buttons");
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("uiButtons");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }

        if (item.itemType == ItemProperties.ItemTypes.Note)
        {
            item.noteAuthor = EditorGUILayout.TextField("Author", item.noteAuthor);
            item.noteDate = EditorGUILayout.TextField("Date", item.noteDate);
            item.noteDescription = EditorGUILayout.TextField("Contents", item.noteDescription);
        }
        else if (item.itemType == ItemProperties.ItemTypes.Consumable)
        {
            item.consumableType = (ItemProperties.ConsumableTypes)EditorGUILayout.EnumPopup("Consumable Type", item.consumableType);

            switch (item.consumableType)
            {
                case ItemProperties.ConsumableTypes.Battery:
                    item.batteryCapacity = EditorGUILayout.IntField("Battery Capacity (In Seconds)", item.batteryCapacity);
                    item.batteryAmount = EditorGUILayout.IntField("Battery Amount (In Seconds)", item.batteryAmount);
                    break;

                case ItemProperties.ConsumableTypes.Healing:
                    item.healAmount = EditorGUILayout.IntField("Heal Amount", item.healAmount);
                    break;
            }

            item.stackable = EditorGUILayout.Toggle("Stackable", item.stackable);

            if (item.stackable)
                item.stackAmount = EditorGUILayout.IntField("Stack Amount", item.stackAmount);
        }
        else if (item.itemType == ItemProperties.ItemTypes.Tool)
        {
            item.toolType = (ItemProperties.WeaponTypes)EditorGUILayout.EnumPopup("Tool Type", item.toolType);
            item.damage = EditorGUILayout.IntField("Damage", item.damage);
            item.wieldTime = EditorGUILayout.FloatField("Wield Time", item.wieldTime);
            item.unWieldTime = EditorGUILayout.FloatField("Unwield Time", item.unWieldTime);
        }
    }
}

#endif