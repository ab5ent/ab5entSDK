#if UNITY_EDITOR

using ab5entSDK.Common.Systems;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveSystem<>), true)]
public class SaveSystemEditor : Editor
{
    private SerializedProperty _fileNameProperty;
    private SerializedProperty _saveTypeProperty;
    private SerializedProperty _keyProperty;
    private SerializedProperty _dataProperty;

    private void OnEnable()
    {
        _fileNameProperty = serializedObject.FindProperty("filename");
        _saveTypeProperty = serializedObject.FindProperty("saveType");
        _keyProperty = serializedObject.FindProperty("key");
        _dataProperty = serializedObject.FindProperty("data");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_fileNameProperty, true);
        EditorGUILayout.PropertyField(_saveTypeProperty, true);

        if ((ESaveType)_saveTypeProperty.enumValueIndex == ESaveType.Encrypt)
        {
            EditorGUILayout.PropertyField(_keyProperty, true);
        }


        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(_dataProperty, true);
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(5);

        if (GUILayout.Button("Open Persistent Data Folder"))
        {
            string dataPath = Application.persistentDataPath;
            EditorUtility.RevealInFinder(dataPath);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif