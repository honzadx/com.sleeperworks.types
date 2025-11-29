using AmeWorks.ScriptableFlow.Editor.Helpers;
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
            var offset = 2 - EditorGUIFunctions.GetIndentOffset();
            position.x += EditorGUIUtility.labelWidth + offset;
            position.width -= EditorGUIUtility.labelWidth + offset;
            using (new EditorGUI.DisabledScope(true))
            {
                var displayText = serializableGuid.isValid ? serializableGuid.guid : "<Invalid>";
                EditorGUI.TextField(position, displayText);
            }
        }
    }
}