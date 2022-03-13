using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ItemProperties))]
public class ItemPropertiesEditor : Editor
{
    SerializedObject editor;

    private void OnEnable()
    {
        editor = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        editor.Update();

        ItemProperties item = (ItemProperties)target;

        item.itemType = (ItemProperties.ItemTypes)EditorGUILayout.EnumPopup("Item Type", item.itemType);
        item.activateType = (ItemProperties.ActivationTypes)EditorGUILayout.EnumPopup("Activiation Type", item.activateType);
        item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
        item.itemDescription = EditorGUILayout.TextField("Item Description", item.itemDescription);

        item.itemPrefab = EditorGUILayout.ObjectField("Item Prefab", item.itemPrefab, typeof(GameObject)) as GameObject;
        item.itemSprite = EditorGUILayout.ObjectField("Item Sprite", item.itemSprite, typeof(Sprite)) as Sprite;

        item.chestButtons = EditorGUILayout.Toggle("Use Chest Buttons", item.chestButtons);

        if(item.chestButtons || item.inventoryButtons)
        {
            item.button = EditorGUILayout.ObjectField("Inventory Buttons", item.button, typeof(GameObject)) as GameObject;
        }

        if (item.chestButtons)
        {
            EditorGUILayout.LabelField("Chest Buttons");
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("uiChestButtons");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }

        item.inventoryButtons = EditorGUILayout.Toggle("Use Inventory Buttons", item.inventoryButtons);

        if (item.inventoryButtons)
        {
            EditorGUILayout.LabelField("Inventory Buttons");
            var inventoryObj = new SerializedObject(target);
            var invProp = inventoryObj.FindProperty("uiInventoryButtons");
            inventoryObj.Update();
            EditorGUILayout.PropertyField(invProp, true);
            inventoryObj.ApplyModifiedProperties();
        }

        item.stackable = EditorGUILayout.Toggle("Stackable", item.stackable);

        if (item.stackable)
        {
            if (item.stackable)
                item.stackAmount = EditorGUILayout.IntField("Stack Amount", item.stackAmount);
        }

        item.controllable = EditorGUILayout.Toggle("Controllable", item.controllable);

        if (item.controllable)
        {
            item.equipTime = EditorGUILayout.FloatField("Equip Time", item.equipTime);
        }

        if (item.itemType == ItemProperties.ItemTypes.Note)
        {
            item.noteSprite = EditorGUILayout.ObjectField("Note Sprite", item.noteSprite, typeof(Sprite)) as Sprite;
        }

        EditorUtility.SetDirty(target);
        editor.ApplyModifiedProperties();
    }
}

#endif