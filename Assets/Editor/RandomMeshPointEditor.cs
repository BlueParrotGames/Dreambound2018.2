using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BPS.Tools.Geometry
{
    [CustomEditor(typeof(RandomMeshPoint))]
    public class RandomMeshPointEditor : Editor
    {

        public bool pointsAvailable;
        public bool displayHandles;
        Vector2 scrollPos;

        RandomMeshPoint r;


        private void OnEnable()
        {
            r = (RandomMeshPoint)target;
        }

        public override void OnInspectorGUI()
        {

            if (r.points.Count > 0) { pointsAvailable = true; }
            else { pointsAvailable = false; }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("filter"));

            GUILayout.Space(5);
            GUILayout.Label("Modifiers", EditorStyles.boldLabel);
            EditorGUILayout.FloatField("Result", r.result);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
            GUILayout.Space(5);
            r.density = EditorGUILayout.Slider("Density", r.density, 0, 1);
            r.radius = EditorGUILayout.Slider("Radius", r.radius, 0.2f, 0.8f);
            GUILayout.Space(10);

            GUILayout.Label("Objects to spawn", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("objects"), true);
            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(10);

            GUILayout.Label("Tools", EditorStyles.boldLabel);
            displayHandles = EditorGUILayout.Toggle("Display Radius", displayHandles);
            GUILayout.BeginHorizontal();

            GUIStyle calculateStyle = new GUIStyle(GUI.skin.button);
            Color calColor;
            ColorUtility.TryParseHtmlString("#457ebc", out calColor);
            calculateStyle.normal.background = MakeTex(300, 1, calColor);

            GUIStyle deleteStyle = new GUIStyle(GUI.skin.button);
            Color delColor;
            ColorUtility.TryParseHtmlString("#ff6060", out delColor);
            deleteStyle.normal.background = MakeTex(300, 1, delColor);

            GUIStyle spawnStyle = new GUIStyle(GUI.skin.button);
            Color spawnColor;
            ColorUtility.TryParseHtmlString("#5fef69", out spawnColor);
            spawnStyle.normal.background = MakeTex(300, 1, spawnColor);

            if (GUILayout.Button("Calculate points", calculateStyle))
            {
                Debug.ClearDeveloperConsole();
                r.CalculatePoints();
            }

            if (GUILayout.Button("Clear points", deleteStyle))
            {
                r.points.Clear();
            }

            SceneView.RepaintAll();
            GUILayout.EndHorizontal();

            if (pointsAvailable && GUILayout.Button("Spawn trees", spawnStyle))
            {
                r.SpawnObjects();
            }

            GUI.color = Color.green;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("points"), true);
        }

        private void OnSceneGUI()
        {
            if(displayHandles)
            {
                foreach (Vector3 p in r.points)
                {
                    Handles.DrawWireDisc(p, Vector3.up, r.radius);
                }
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}