/*
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Wave))]
public class SquadronEditor : Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("WaveEvents"),
                true, true, true, true);

        list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 2;
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("SpawnPattern"), GUIContent.none);
                    EditorGUI.PropertyField(
                        new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("SpawnZone"), GUIContent.none);
                    EditorGUI.PropertyField(
                        new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("EnemyType"), GUIContent.none);
                    EditorGUI.PropertyField(
                        new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("Count"), GUIContent.none);
                };

        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, serializedObject.FindProperty("MissionName") != null ? serializedObject.FindProperty("MissionName").ToString() : "Mission Waves");
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