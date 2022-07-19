using Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(NamedPropertyAttribute))]
    public class NamedPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            
            try 
            {
                EditorGUI.PropertyField(rect, property, new GUIContent(((NamedPropertyAttribute) attribute).name ), true);
            } 
            catch 
            {
                EditorGUI.PropertyField(rect, property, label, true);
            }
            
            EditorGUI.EndProperty();
        }
    }
}