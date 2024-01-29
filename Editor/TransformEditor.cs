#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace ab5entSDK
{
    [CustomEditor(typeof(Transform))]
    public class TransformEditor : Editor
    {
        private static bool foldoutHelper = true;

        private static Vector3 position;
        private static Quaternion rotation;
        private static Vector3 scale;

        private SerializedProperty positionProperty;
        private SerializedProperty rotationProperty;
        private SerializedProperty scaleProperty;

        private Transform transform;

        private void OnEnable()
        {
            positionProperty = serializedObject.FindProperty("m_LocalPosition");
            rotationProperty = serializedObject.FindProperty("m_LocalRotation");
            scaleProperty = serializedObject.FindProperty("m_LocalScale");
        }

        public override void OnInspectorGUI()
        {
            if (!transform)
            {
                transform = (Transform)target;
            }

            DrawHeader("Location");

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("World Position", transform.position);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(positionProperty, new GUIContent("Local Position"));

            DrawHeader("Rotation");

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("World Rotation", transform.rotation.eulerAngles);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(rotationProperty, new GUIContent("Local Rotation"));

            DrawHeader("Scale");

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("World Scale", transform.lossyScale);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(scaleProperty, new GUIContent("Local Scale"));

            GUILayout.Space(10);
            foldoutHelper = EditorGUILayout.Foldout(foldoutHelper, "Copy And Parse");

            GUILayout.BeginHorizontal();

            ProcessCopy();

            GUILayout.Space(10);

            ProcessParse();

            GUILayout.EndHorizontal();



            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeader(string label)
        {
            GUILayout.Space(10);
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14
            };
            EditorGUILayout.LabelField(label, style);
        }

        private void ProcessCopy()
        {
            if (foldoutHelper)
            {
                GUILayout.BeginVertical();

                if (GUILayout.Button("Copy Position"))
                {
                    position = transform.position;
                }

                if (GUILayout.Button("Copy Rotation"))
                {
                    rotation = transform.rotation;
                }

                if (GUILayout.Button("Copy Scale"))
                {
                    scale = transform.localScale;
                }

                if (GUILayout.Button("Copy Component"))
                {
                    position = transform.position;
                    rotation = transform.rotation;
                    scale = transform.localScale;
                }

                GUILayout.EndVertical();
            }
        }

        private void ProcessParse()
        {
            if (foldoutHelper)
            {
                GUILayout.BeginVertical();

                if (GUILayout.Button($"Parse Position ({position.x},{position.y},{position.z})"))
                {
                    transform.position = position;
                }

                if (GUILayout.Button($"Parse Rotation ({rotation.eulerAngles.x},{rotation.eulerAngles.y},{rotation.eulerAngles.z})"))
                {
                    transform.rotation = rotation;
                }

                if (GUILayout.Button($"Parse Scale ({scale.x},{scale.y},{scale.z})"))
                {
                    transform.localScale = scale;
                }


                if (GUILayout.Button("Parse Component"))
                {
                    transform.position = position;
                    transform.rotation = rotation;
                    transform.localScale = scale;
                }

                GUILayout.EndVertical();
            }
        }
    }
}

#endif