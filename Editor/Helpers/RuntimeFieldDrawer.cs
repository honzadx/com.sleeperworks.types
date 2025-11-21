using System;
using UnityEditor;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Editor.Helpers
{
    // Use for non serialized values
    public static class RuntimeFieldDrawer<T>
    {
        private static readonly Func<string, T, T> s_drawer;
        
        static RuntimeFieldDrawer()
        {
            if (System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (typeof(T) == typeof(AnimationCurve))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.CurveField(label, (AnimationCurve)(object)value);
                else if (typeof(T) == typeof(Gradient))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.GradientField(label, (Gradient)(object)value);
                else if (typeof(T) == typeof(string))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.TextField(label, (string)(object)value);
                else if (typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.ObjectField(label, (UnityEngine.Object)(object)value, typeof(T), allowSceneObjects: true);
            }
            else
            {
                if (typeof(T) == typeof(int))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.IntField(label, (int)(object)value);
                else if (typeof(T) == typeof(float))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.FloatField(label, (float)(object)value);
                else if (typeof(T) == typeof(bool))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.Toggle(label, (bool)(object)value);
                else if (typeof(T) == typeof(Color))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.ColorField(label, (Color)(object)value);
                else if (typeof(T) == typeof(Vector2))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.Vector2Field(label, (Vector2)(object)value);
                else if (typeof(T) == typeof(Vector2Int))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.Vector2IntField(label, (Vector2Int)(object)value);
                else if (typeof(T) == typeof(Vector3))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.Vector3Field(label, (Vector3)(object)value);
                else if (typeof(T) == typeof(Vector3Int))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.Vector3IntField(label, (Vector3Int)(object)value);
                else if (typeof(T) == typeof(Vector4))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.Vector4Field(label, (Vector4)(object)value);
                else if (typeof(T) == typeof(Rect))
                    s_drawer = (label, value) => (T)(object)EditorGUILayout.RectField(label, (Rect)(object)value);
                else if (typeof(T) == typeof(Quaternion))
                    s_drawer = (label, value) =>
                    {
                        var q = (Quaternion)(object)value;
                        var euler = EditorGUILayout.Vector3Field(label, q.eulerAngles);
                        return (T)(object)Quaternion.Euler(euler);
                    };
                else if (typeof(T) == typeof(Bounds))
                    s_drawer = (label, value) =>
                    {
                        var b = (Bounds)(object)value;
                        EditorGUILayout.LabelField(label);
                        b.center = EditorGUILayout.Vector3Field("Center", b.center);
                        b.size = EditorGUILayout.Vector3Field("Size", b.size);
                        return (T)(object)b;
                    };
            }
            s_drawer ??= (label, value) =>
            {
                EditorGUILayout.LabelField(label, $"Unsupported type {typeof(T).Name}");
                return value;
            };
        }

        public static T DrawField(string label, T value)
        {
            return s_drawer(label, value);
        }
    }
}