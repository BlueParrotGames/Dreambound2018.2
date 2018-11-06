using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomMeshPoint))]
public class RandomMeshPointEditor : Editor
{
    SerializedProperty filter;
    SerializedProperty points;
    SerializedProperty result;
    SerializedProperty density;

    private void OnEnable()
    {
        filter = serializedObject.FindProperty("filter");
        points = serializedObject.FindProperty("points");
        result = serializedObject.FindProperty("result");
        density = serializedObject.FindProperty("density");
    }

    public override void OnInspectorGUI()
    {
        RandomMeshPoint r = (RandomMeshPoint)target;

        EditorGUILayout.PropertyField(filter);
        GUILayout.Space(5);
        r.result = EditorGUILayout.FloatField("Result", r.result);
        r.density = EditorGUILayout.Slider(r.density, 0, 1);
        EditorGUILayout.PropertyField(points, true);
        GUILayout.Space(5);

        GUILayout.Label("Tools");
        GUILayout.BeginHorizontal();

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.textColor = Color.black;

        Color calColor;
        ColorUtility.TryParseHtmlString("#457ebc", out calColor);
        GUI.color = calColor;
        if (GUILayout.Button("Calculate points", buttonStyle))
        {
            Debug.ClearDeveloperConsole();
            r.CalculatePoints();
        }

        Color delColor;
        ColorUtility.TryParseHtmlString("#ff6060", out delColor);
        GUI.color = delColor;
        if (GUILayout.Button("Clear points", buttonStyle))
        {
            r.points.Clear();
        }

        SceneView.RepaintAll();
        GUILayout.EndHorizontal();
    }
}