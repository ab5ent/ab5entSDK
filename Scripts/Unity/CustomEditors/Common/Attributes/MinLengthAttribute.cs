using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace ab5entSDK.Unity.Editor.Attributes
{
    public class MinLengthAttribute : PropertyAttribute
    {
        public int MinLength { get; private set; }

        public MinLengthAttribute(int length)
        {
            MinLength = length;
        }
    }
    
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(MinLengthAttribute))]
    public class MinLengthAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinLengthAttribute minLengthAttribute = attribute as MinLengthAttribute;

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
                }

                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use MinLength attribute with string fields.");
            }
        }
    }

#endif
}