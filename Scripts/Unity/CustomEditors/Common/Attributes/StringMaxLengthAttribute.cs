using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace ab5entSDK.Unity.Editor.Attributes
{
    public class StringMaxLengthAttribute : PropertyAttribute
    {
        public int MaxLength { get; private set; }

        public StringMaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(StringMaxLengthAttribute))]
    public class StringMaxLengthAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            StringMaxLengthAttribute maxLengthAttribute = attribute as StringMaxLengthAttribute;

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

                    label.tooltip += $" [max: {maxLength}]";
                }

                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, $"Use {typeof(StringMaxLengthAttribute).Name} attribute with string fields.");
            }
        }
    }

#endif

}