#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace ab5entSDK
{
    [CustomEditor(typeof(Transform))]
    public class TransformEditor : Editor
    {
        private static bool isFoldout = true;

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

            ProcessPosition();
            ProcessRotation();
            ProcessScale();

            ProcessExtensions();

            serializedObject.ApplyModifiedProperties();
        }

        private void ProcessPosition()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("World Position", transform.position);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(positionProperty, new GUIContent("LOCAL POSITION"));
        }

        private void ProcessRotation()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("World Rotation", transform.rotation.eulerAngles);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(rotationProperty, new GUIContent("LOCAL ROTATION"));
        }

        private void ProcessScale()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("World Scale", transform.lossyScale);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(scaleProperty, new GUIContent("LOCAL SCALE"));
        }

        private void ProcessExtensions()
        {
            isFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(isFoldout, "Copy And Parse");

            if (isFoldout)
            {
                GUILayout.BeginHorizontal("GroupBox");
                ProcessCopy();
                ProcessParse();
                GUILayout.EndHorizontal();

                ProcessReset();

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

        private void ProcessCopy()
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

        private void ProcessParse()
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
                transform.SetPositionAndRotation(position, rotation);
                transform.localScale = scale;
            }

            GUILayout.EndVertical();
        }

        private void ProcessReset()
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("Reset"))
            {
                transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                transform.localScale = Vector3.one;
            }

            GUILayout.EndVertical();
        }
    }
}

#endif