#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace ab5entSDK.Unity.Editor
{
    [CustomEditor(typeof(Transform))]
    public class TransformEditor : UnityEditor.Editor
    {
        private static bool _isFoldout = true;

        private static Vector3 _position;
        private static Quaternion _rotation;
        private static Vector3 _scale;

        private SerializedProperty _positionProperty;
        private SerializedProperty _rotationProperty;
        private SerializedProperty _scaleProperty;

        private Transform _transform;

        private void OnEnable()
        {
            _positionProperty = serializedObject.FindProperty("m_LocalPosition");
            _rotationProperty = serializedObject.FindProperty("m_LocalRotation");
            _scaleProperty = serializedObject.FindProperty("m_LocalScale");
        }

        public override void OnInspectorGUI()
        {
            if (!_transform)
            {
                _transform = (Transform)target;
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
            EditorGUILayout.Vector3Field("World Position", _transform.position);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(_positionProperty, new GUIContent("LOCAL POSITION"));
        }

        private void ProcessRotation()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("World Rotation", _transform.rotation.eulerAngles);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(_rotationProperty, new GUIContent("LOCAL ROTATION"));
        }

        private void ProcessScale()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3Field("World Scale", _transform.lossyScale);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(_scaleProperty, new GUIContent("LOCAL SCALE"));
        }

        private void ProcessExtensions()
        {
            _isFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_isFoldout, "Copy And Parse");

            if (!_isFoldout)
            {
                return;
            }
            
            GUILayout.BeginHorizontal("GroupBox");
            
            ProcessCopy();
            ProcessParse();
            
            GUILayout.EndHorizontal();

            ProcessReset();

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void ProcessCopy()
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("Copy World Position"))
            {
                _position = _transform.position;
            }

            if (GUILayout.Button("Copy World Rotation"))
            {
                _rotation = _transform.rotation;
            }

            if (GUILayout.Button("Copy World Scale"))
            {
                _scale = _transform.localScale;
            }

            if (GUILayout.Button("Copy World Component"))
            {
                _position = _transform.position;
                _rotation = _transform.rotation;
                _scale = _transform.localScale;
            }

            GUILayout.EndVertical();
        }

        private void ProcessParse()
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button($"Parse World Position ({_position.x},{_position.y},{_position.z})"))
            {
                _transform.position = _position;
            }

            if (GUILayout.Button($"Parse World Rotation ({_rotation.eulerAngles.x},{_rotation.eulerAngles.y},{_rotation.eulerAngles.z})"))
            {
                _transform.rotation = _rotation;
            }

            if (GUILayout.Button($"Parse World Scale ({_scale.x},{_scale.y},{_scale.z})"))
            {
                _transform.localScale = _scale;
            }

            if (GUILayout.Button("Parse World Component"))
            {
                _transform.SetPositionAndRotation(_position, _rotation);
                _transform.localScale = _scale;
            }

            GUILayout.EndVertical();
        }

        private void ProcessReset()
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("Reset"))
            {
                _transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                _transform.localScale = Vector3.one;
            }

            GUILayout.EndVertical();
        }
    }
}

#endif
