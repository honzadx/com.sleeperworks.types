using System;
using AmeWorks.ScriptableFlow.Editor.Helpers;
using AmeWorks.ScriptableFlow.Runtime.Types;
using UnityEditor;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Editor.PropertyDrawers
{
    public class ValueReferencePropertyDrawer<TRef, TSO, TType> : PropertyDrawer where TRef : ValueReferenceT<TType> where TSO : ValueSOT<TType>
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var reference = (TRef)property.boxedValue;
            var fullWidth = position.width;
            
            position.width = EditorGUIUtility.labelWidth;
            EditorGUI.LabelField(position, label);
            
            position.x += position.width;
            position.width = 20f;
            var icon = EditorGUIUtility.IconContent(
                reference._useScriptableReference
                    ? "Toolbar Minus"
                    : "Toolbar Plus");
            if (GUI.Button(position, icon))
            {
                reference._useScriptableReference = !reference._useScriptableReference;
            }
            
            position.x += position.width + 2;
            position.width = fullWidth - (EditorGUIUtility.labelWidth + 22f);
            if (reference._useScriptableReference)
            {
                reference._scriptableValue = (TSO)EditorGUI.ObjectField(position, reference._scriptableValue, typeof(TSO), false);
                
                if (reference._scriptableValue)
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        if (EditorApplication.isPlaying)
                            reference._scriptableValue._runtimeValue = FieldDrawer<TType>.DrawField("Value", reference._scriptableValue._runtimeValue);
                        else
                            reference._scriptableValue._defaultValue = FieldDrawer<TType>.DrawField("Value", reference._scriptableValue._defaultValue);
                    }
                }
            }
            else
            {
                reference._value = FieldDrawer<TType>.DrawField(position, string.Empty, reference._value);
            }
            property.boxedValue = reference;
        }
    }
    
    [CustomPropertyDrawer(typeof(BoolReference))]
    public class BoolReferencePropertyDrawer : ValueReferencePropertyDrawer<BoolReference, BoolSO, bool> { }
    
    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferencePropertyDrawer : ValueReferencePropertyDrawer<FloatReference, FloatSO, float> { }
    
    [CustomPropertyDrawer(typeof(IntReference))]
    public class IntReferencePropertyDrawer : ValueReferencePropertyDrawer<IntReference, IntSO, int> { }
    
    [CustomPropertyDrawer(typeof(StringReference))]
    public class StringReferencePropertyDrawer : ValueReferencePropertyDrawer<StringReference, StringSO, string> { }
    
    [CustomPropertyDrawer(typeof(ColorReference))]
    public class ColorReferencePropertyDrawer : ValueReferencePropertyDrawer<ColorReference, ColorSO, Color> { }
    
    [CustomPropertyDrawer(typeof(CurveReference))]
    public class CurveReferencePropertyDrawer : ValueReferencePropertyDrawer<CurveReference, CurveSO, AnimationCurve> { }
    
    [CustomPropertyDrawer(typeof(GradientReference))]
    public class GradientReferencePropertyDrawer : ValueReferencePropertyDrawer<GradientReference, GradientSO, Gradient> { }

    [CustomPropertyDrawer(typeof(Vector2Reference))]
    public class Vector2ReferencePropertyDrawer : ValueReferencePropertyDrawer<Vector2Reference, Vector2SO, Vector2> { }
    
    [CustomPropertyDrawer(typeof(Vector2IntReference))]
    public class Vector2IntReferencePropertyDrawer : ValueReferencePropertyDrawer<Vector2IntReference, Vector2IntSO, Vector2Int> { }
    
    [CustomPropertyDrawer(typeof(Vector3Reference))]
    public class Vector3ReferencePropertyDrawer : ValueReferencePropertyDrawer<Vector3Reference, Vector3SO, Vector3> { }
    
    [CustomPropertyDrawer(typeof(Vector3IntReference))]
    public class Vector3IntReferencePropertyDrawer : ValueReferencePropertyDrawer<Vector3IntReference, Vector3IntSO, Vector3Int> { }
    
    [CustomPropertyDrawer(typeof(Vector4Reference))]
    public class Vector4ReferencePropertyDrawer : ValueReferencePropertyDrawer<Vector4Reference, Vector4SO, Vector4> { }
}