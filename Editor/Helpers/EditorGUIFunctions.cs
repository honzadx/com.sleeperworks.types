using System;
using UnityEditor;
using UnityEngine;

namespace ScriptableFlow.Editor
{
    public static class EditorGUIFunctions
    {
        public static object DrawField(string label, object value, Type type,
            EditorTypeHint typeHint = EditorTypeHint.None)
        {
            // Fast path with hints
            if (typeHint != EditorTypeHint.None)
            {
                return typeHint switch
                {
                    EditorTypeHint.Bool => EditorGUILayout.Toggle(label, (bool)value),
                    EditorTypeHint.Float => EditorGUILayout.FloatField(label, (float)value),
                    EditorTypeHint.Int => EditorGUILayout.IntField(label, (int)value),
                    EditorTypeHint.String => EditorGUILayout.TextField(label, (string)value),
                    EditorTypeHint.Curve => EditorGUILayout.CurveField(label, (AnimationCurve)value),
                    EditorTypeHint.Color => EditorGUILayout.ColorField(label, (Color)value),
                    EditorTypeHint.Gradient => EditorGUILayout.GradientField(label, (Gradient)value),
                    EditorTypeHint.UnityObject => EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, type,
                        allowSceneObjects: true),
                    EditorTypeHint.Vector2 => EditorGUILayout.Vector2Field(label, (Vector2)value),
                    EditorTypeHint.Vector3 => EditorGUILayout.Vector3Field(label, (Vector3)value),
                    _ => throw new NotImplementedException($"{typeHint.ToString()} is not implemented."),
                };
            }

            // Slow fallback path
            if (type.IsClass)
            {
                if (type == typeof(AnimationCurve))
                    return EditorGUILayout.CurveField(label, (AnimationCurve)value);
                if (type == typeof(Gradient))
                    return EditorGUILayout.GradientField(label, (Gradient)value);
                if (type == typeof(string))
                    return EditorGUILayout.TextField(label, (string)value);
                if (type.IsSubclassOf(typeof(UnityEngine.Object)))
                    return EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, type, allowSceneObjects: true);
            }

            if (type == typeof(int))
                return EditorGUILayout.IntField(label, (int)value);
            if (type == typeof(float))
                return EditorGUILayout.FloatField(label, (float)value);
            if (type == typeof(bool))
                return EditorGUILayout.Toggle(label, (bool)value);
            if (type == typeof(Color))
                return EditorGUILayout.ColorField(label, (Color)value);
            if (type == typeof(Vector2))
                return EditorGUILayout.Vector2Field(label, (Vector2)value);
            if (type == typeof(Vector3))
                return EditorGUILayout.Vector3Field(label, (Vector3)value);

            EditorGUILayout.LabelField($"{label}: (Unsupported type {type.Name})");
            return value;
        }

        public static void DrawHeaderText(string text)
        {
            EditorGUILayout.LabelField(text, EditorGUIStatics.s_headerStyle);
        }

        public static void DrawDecayMeter(string label, float lastSignalTime, float decayTime = EditorGUIStatics.DEFAULT_DECAY_METER_TIME)
        {
            float progress = 1 - Mathf.Clamp01(((float)EditorApplication.timeSinceStartup - lastSignalTime) / decayTime);
            Rect progressBarRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, 20);
            EditorGUI.ProgressBar(progressBarRect, progress, label);
        }

        public static bool DrawRuntimeEditToggle(ref bool allowRuntimeEdit)
        {
            if (Application.isPlaying)
                allowRuntimeEdit = EditorGUILayout.Toggle("Runtime Edit", allowRuntimeEdit);
            return Application.isPlaying & allowRuntimeEdit;
        }
    }
}