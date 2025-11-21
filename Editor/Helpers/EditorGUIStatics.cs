using UnityEditor;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Editor.Helpers
{
    internal static class EditorGUIStatics
    {
        public static readonly Color s_darkBackground = new(0.1f, 0.1f, 0.1f);
        public static readonly GUIStyle s_headerText = new(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
    }
}