using AmeWorks.ScriptableFlow.Runtime;
using UnityEditor;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(SerializableGuid))]
    public class SerializableGuidPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var serializableGuid = (SerializableGuid)property.boxedValue;
            EditorGUI.LabelField(position, label);
            position.x += EditorGUIUtility.labelWidth + 2;
            position.width -= EditorGUIUtility.labelWidth + 2;
            using (new EditorGUI.DisabledScope(true))
            {
                var displayText = serializableGuid.isValid ? serializableGuid.guid : "<Invalid>";
                EditorGUI.TextField(position, displayText);
            }
        }
    }
}