using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace ab5entSDK.Unity.Editor.Attributes
{
    public class MaxLengthAttribute : PropertyAttribute
    {
        public int MaxLength { get; private set; }

        public MaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(MaxLengthAttribute))]
    public class MaxLengthAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MaxLengthAttribute maxLengthAttribute = attribute as MaxLengthAttribute;

            if (property.propertyType == SerializedPropertyType.String)
            {
                if (maxLengthAttribute != null)
                {
                    string value = property.stringValue;

                    int maxLength = maxLengthAttribute.MaxLength;

                    if (value.Length > maxLength)
                    {
                        value = value[0..maxLength];
                        property.stringValue = value;
                    }
                }

                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use MaxLength attribute with string fields.");
            }
        }
    }

#endif

}