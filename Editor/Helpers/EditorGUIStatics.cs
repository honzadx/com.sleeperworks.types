using UnityEditor;
using UnityEngine;

internal static class EditorGUIStatics
{
    public const float DEFAULT_DECAY_METER_TIME = 0.25f;
    public const float MAX_LIST_VIEW_SIZE = 100;
    
    public static readonly GUIStyle s_headerStyle = new(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
    public static readonly Color s_debugBackgroundColor = new(0.1f, 0.1f, 0.1f, 0.75f);
}