using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace ab5entSDK.Unity.ExtendedEditor.Attributes
{
    public class StringMinLengthAttribute : PropertyAttribute
    {
        public int MinLength { get; private set; }

        public StringMinLengthAttribute(int minLength)
        {
            MinLength = minLength;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(StringMinLengthAttribute))]
    public class StringMinLengthAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            StringMinLengthAttribute minLengthAttribute = attribute as StringMinLengthAttribute;

            if (property.propertyType == SerializedPropertyType.String)
            {
                if (minLengthAttribute != null)
                {
                    string value = property.stringValue;

                    int minLength = minLengthAttribute.MinLength;

                    if (value.Length < minLength)
                    {
                        value = value.PadRight(minLength, '_');
                        property.stringValue = value;
                    }

                    label.tooltip += $" [min: {minLength}]";
                }

                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, $"Use {typeof(StringMinLengthAttribute).Name} attribute with string fields.");
            }
        }
    }

#endif

}