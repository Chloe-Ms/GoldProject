using System;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(HeroesSensibility))]
public class HeroesSensibilityEditor : Editor
{
    private HeroesSensibility _heroesSensibilitiesSO;
    private SerializedProperty _propSensibilities;
    bool modified = false;

    private void OnEnable()
    {
        _heroesSensibilitiesSO = target as HeroesSensibility;
        _propSensibilities = serializedObject.FindProperty("heroesSensibilities");
    }

    public override void OnInspectorGUI()
    {
        int columns = Enum.GetNames(typeof(Effect)).Length;
        int rows = Enum.GetNames(typeof(Role)).Length;

        if (!modified && _heroesSensibilitiesSO.heroesSensibilities.Length == 0)
        {
            modified = true;
            _heroesSensibilitiesSO.heroesSensibilities = new int[columns * rows];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    _heroesSensibilitiesSO.heroesSensibilities[i + (j * columns)] = 0;
                }
            }

        }
        // Update serialized object
        serializedObject.Update();

        GUIStyle tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        GUIStyle rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;

        GUIStyle rowHeaderStyle = new GUIStyle();
        rowHeaderStyle.fixedWidth = 65;
        rowHeaderStyle.fixedHeight = 25.5f;
        GUIStyle columnHeaderStyle = new GUIStyle();
        columnHeaderStyle.fixedWidth = 80;
        columnHeaderStyle.fixedHeight = 25.5f;

        GUIStyle columnLabelStyle = new GUIStyle();
        columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
        columnLabelStyle.alignment = TextAnchor.MiddleCenter;
        columnLabelStyle.fontStyle = FontStyle.Bold;

        GUIStyle cornerLabelStyle = new GUIStyle();
        cornerLabelStyle.fixedWidth = 65;
        cornerLabelStyle.alignment = TextAnchor.MiddleRight;
        cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
        cornerLabelStyle.fontSize = 14;
        cornerLabelStyle.padding.top = -5;

        GUIStyle rowLabelStyle = new GUIStyle();
        rowLabelStyle.fixedWidth = 65;
        rowLabelStyle.alignment = TextAnchor.MiddleRight;
        rowLabelStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.BeginHorizontal(tableStyle);
        for (int j = -1; j < rows; j++)
        {

            EditorGUILayout.BeginVertical();
            for (int i = -1; i < columns; i++)
            {
                if (i != (int)Effect.NONE)
                {
                    if (j >= 0 && i >= 0)
                    {
                        EditorGUILayout.BeginHorizontal(rowStyle);
                        SerializedProperty element = _propSensibilities.GetArrayElementAtIndex(i + (j * columns));
                        EditorGUILayout.PropertyField(element, GUIContent.none);
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (j >= 0 && i == -1)
                    {
                        EditorGUILayout.BeginVertical(columnHeaderStyle);
                        EditorGUILayout.LabelField(((Role)j).ToString());
                        EditorGUILayout.EndHorizontal();
                    }
                    else if (i >= 0 && j == -1)
                    {
                        EditorGUILayout.BeginVertical(rowHeaderStyle);
                        EditorGUILayout.LabelField(((Effect)i).ToString());
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.BeginVertical(rowHeaderStyle);
                        EditorGUILayout.LabelField("");
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndVertical();
            
        }
        EditorGUILayout.EndHorizontal();
        // Apply changes to serialized object
        serializedObject.ApplyModifiedProperties();
    }
}
#endif