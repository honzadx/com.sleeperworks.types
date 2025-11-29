using UnityEditor;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Editor.Helpers
{
    public static class EditorGUIFunctions
    {
        public static float GetIndentOffset()
        {
            return EditorGUI.indentLevel * 15f;
        }
        
        public static void DrawMonoScript(ScriptableObject target)
        {
            MonoScript script = MonoScript.FromScriptableObject(target);
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            }
        }
        
        public static void DrawHeaderText(string text)
        {
            var rect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(rect, EditorGUIStatics.s_darkBackground);
            EditorGUILayout.LabelField(text, EditorGUIStatics.s_headerText);
            EditorGUILayout.EndVertical();
        }
    }
}