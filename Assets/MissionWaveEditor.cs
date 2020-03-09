/*
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(WaveEvent))]
public class MissionWaveEditor : Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("_waveEvents"),
                true, true, true, true);

        list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("_duration"), GUIContent.none);
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("_pattern"), GUIContent.none);
                    EditorGUI.PropertyField(
                        new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("_zone"), GUIContent.none);
                    EditorGUI.PropertyField(
                        new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("_enemyType"), GUIContent.none);
                    EditorGUI.PropertyField(
                        new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("_count"), GUIContent.none);
                };

        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Wave");
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
*/