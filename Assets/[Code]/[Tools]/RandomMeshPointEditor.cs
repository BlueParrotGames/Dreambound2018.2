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
    SerializedProperty objects;

    public static bool pointsAvailable;
    Vector2 scrollPos;

    RandomMeshPoint r;


    private void OnEnable()
    {
        filter = serializedObject.FindProperty("filter");
        points = serializedObject.FindProperty("points");
        result = serializedObject.FindProperty("result");
        density = serializedObject.FindProperty("density");
        objects = serializedObject.FindProperty("objects");

        r = (RandomMeshPoint)target;
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.PropertyField(filter);

            GUILayout.Space(5);
        GUILayout.Label("Modifiers", EditorStyles.boldLabel);
        EditorGUILayout.FloatField("Result", r.result);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
            GUILayout.Space(5);
        r.density = EditorGUILayout.Slider("Density", r.density, 0, 1);
        r.radius = EditorGUILayout.Slider("Radius", r.radius, 0.2f, 0.8f);
            GUILayout.Space(10);

        GUILayout.Label("Objects to spawn", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(objects, true);
        serializedObject.ApplyModifiedProperties();
            GUILayout.Space(10);


        GUILayout.Label("Tools", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(points, true);
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

        Color spawnColor;
        ColorUtility.TryParseHtmlString("#42f489", out spawnColor);
        GUI.color = spawnColor;
        if (pointsAvailable && GUILayout.Button("Spawn trees"))
        {
            r.SpawnObjects();
        }
    }

    private void OnSceneGUI()
    {
        foreach(Vector3 p in r.points)
        {
            Handles.DrawWireDisc(p, Vector3.up, r.radius);
        }
    }
}