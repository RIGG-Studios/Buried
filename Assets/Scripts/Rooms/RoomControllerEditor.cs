using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(RoomController))]
public class RoomControllerEdidtor : Editor
{
    public override void OnInspectorGUI()
    {
        /*/
        RoomController controller = (RoomController)target;

        controller.room = (Room)EditorGUILayout.ObjectField("Room Properties: ", controller.room, typeof(Room), true);
        controller.mapUIElement = (GameObject)EditorGUILayout.ObjectField("Map UI Element: ", controller.mapUIElement, typeof(GameObject), true);
        controller.useShadow = EditorGUILayout.Toggle("Use Shadow: ", controller.useShadow);

        if(controller.useShadow)
            controller.shadowRender = (GameObject)EditorGUILayout.ObjectField("Shadow Render: ", controller.shadowRender, typeof(GameObject), true);

        if (controller.room != null)
        {
            if (controller.room.roomType == Room.RoomType.SecretRoom)
            {
                SerializedObject obj = new SerializedObject(target);

                EditorGUILayout.PropertyField(obj.FindProperty("blockedPieces"), true);
                EditorGUILayout.PropertyField(obj.FindProperty("entrances"), true);

                obj.ApplyModifiedProperties();
            }
        }
        /*/
    }
}
#endif
