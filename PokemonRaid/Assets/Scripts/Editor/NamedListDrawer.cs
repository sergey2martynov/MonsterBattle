using System;
using Attributes;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomPropertyDrawer(typeof(NamedListAttribute))]
    public class NamedListDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            try
            {
                var pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
                EditorGUI.PropertyField(rect, property, new GUIContent(((NamedListAttribute) attribute).names[pos]), true);
            }
            catch
            {
                EditorGUI.PropertyField(rect, property, label, true);
            }
        }
    }
}