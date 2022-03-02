using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(UIElementGroup))]
public class UIElementGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIElementGroup element = (UIElementGroup)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Find Elements"))
            element.FindUIElements();
    }
}
#endif