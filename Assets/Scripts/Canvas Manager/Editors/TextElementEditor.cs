using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(TextElement))]
public class TextElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TextElement element = (TextElement)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Load Element"))
        {
            element.Setup();
        }
    }
}
#endif
