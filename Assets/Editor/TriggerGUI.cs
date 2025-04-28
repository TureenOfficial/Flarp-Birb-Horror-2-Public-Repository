#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Trigger))]
public class TriggerGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Trigger DropdownParse = (Trigger)target;

        EditorGUILayout.LabelField(" ");
        EditorGUILayout.LabelField("Carry Order: ", EditorStyles.boldLabel);
        GUIContent styleLabel = new GUIContent("Style");
        DropdownParse.styleIndex = EditorGUILayout.Popup(styleLabel, DropdownParse.styleIndex, DropdownParse.Style);
    }
}
#endif