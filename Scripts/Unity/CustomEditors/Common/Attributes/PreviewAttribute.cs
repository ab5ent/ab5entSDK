using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace ab5entSDK.ExtendedEditor.Attributes
{
    public class PreviewAttribute : PropertyAttribute
    {
    }

    #if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool previousGUIState = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = previousGUIState;
        }
    }

    #endif

}