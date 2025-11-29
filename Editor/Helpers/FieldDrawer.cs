using System;
using UnityEditor;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Editor.Helpers
{
    public static class FieldDrawer<T>
    {
        private static readonly Func<string, T, T> s_autoLayoutDrawer;
        private static readonly Func<Rect, string, T, T> s_drawer;
        
        static FieldDrawer()
        {
            if (System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (typeof(T) == typeof(AnimationCurve))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.CurveField(label, (AnimationCurve)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.CurveField(position, label, (AnimationCurve)(object)value);
                }
                else if (typeof(T) == typeof(Gradient))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.GradientField(label, (Gradient)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.GradientField(position, label, (Gradient)(object)value);
                }
                else if (typeof(T) == typeof(string))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.TextField(label, (string)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.TextField(position, label, (string)(object)value);
                }
                else if (typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.ObjectField(label, (UnityEngine.Object)(object)value, typeof(T), allowSceneObjects: true);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.ObjectField(position, label, (UnityEngine.Object)(object)value, typeof(T), allowSceneObjects: true);
                }
            }
            else
            {
                if (typeof(T) == typeof(int))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.IntField(label, (int)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.IntField(position, label, (int)(object)value);
                }
                else if (typeof(T) == typeof(float))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.FloatField(label, (float)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.FloatField(position, label, (float)(object)value);
                }
                else if (typeof(T) == typeof(bool))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.Toggle(label, (bool)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.Toggle(position, label, (bool)(object)value);
                }
                else if (typeof(T) == typeof(Color))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.ColorField(label, (Color)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.ColorField(position, label, (Color)(object)value);
                    
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.Vector2Field(label, (Vector2)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.Vector2Field(position, label, (Vector2)(object)value);
                }
                else if (typeof(T) == typeof(Vector2Int))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.Vector2IntField(label, (Vector2Int)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.Vector2IntField(position, label, (Vector2Int)(object)value);
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.Vector3Field(label, (Vector3)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.Vector3Field(position, label, (Vector3)(object)value);
                }
                else if (typeof(T) == typeof(Vector3Int))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.Vector3IntField(label, (Vector3Int)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.Vector3IntField(position, label, (Vector3Int)(object)value);
                }
                else if (typeof(T) == typeof(Vector4))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.Vector4Field(label, (Vector4)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.Vector4Field(position, label, (Vector4)(object)value);
                }
                else if (typeof(T) == typeof(Rect))
                {
                    s_autoLayoutDrawer = (label, value) => (T)(object)EditorGUILayout.RectField(label, (Rect)(object)value);
                    s_drawer = (position, label, value) => (T)(object)EditorGUI.RectField(position, label, (Rect)(object)value);
                }
                else if (typeof(T) == typeof(Quaternion))
                {
                    s_autoLayoutDrawer = (label, value) =>
                    {
                        var q = (Quaternion)(object)value;
                        var euler = EditorGUILayout.Vector3Field(label, q.eulerAngles);
                        return (T)(object)Quaternion.Euler(euler);
                    };
                    s_drawer = (position, label, value) =>
                    {
                        var q = (Quaternion)(object)value;
                        var euler = EditorGUI.Vector3Field(position, label, q.eulerAngles);
                        return (T)(object)Quaternion.Euler(euler);
                    };
                }
            }
            s_drawer ??= (position, label, value) =>
            {
                EditorGUI.LabelField(position, label, $"Unsupported type {typeof(T).Name}");
                return value;
            };
            s_autoLayoutDrawer ??= (label, value) =>
            {
                EditorGUILayout.LabelField(label, $"Unsupported type {typeof(T).Name}");
                return value;
            };
        }
        public static T DrawField(Rect position, string label, T value)
        {
            return s_drawer(position, label, value);
        }

        public static T DrawField(string label, T value)
        {
            return s_autoLayoutDrawer(label, value);
        }
    }
}