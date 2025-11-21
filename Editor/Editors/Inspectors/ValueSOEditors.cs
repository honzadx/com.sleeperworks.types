using System;
using UnityEditor;
using UnityEngine;
using ScriptableFlow.Runtime;

namespace ScriptableFlow.Editor
{
    public abstract class ValueSOTEditor<T, TValue> : UnityEditor.Editor where T : ValueSOT<TValue>
    {
        private T _target;
        private bool _allowRuntimeEdit;
        private Func<TValue, TValue> _drawValueFieldFunction;
        private SerializedProperty _defaultValueProperty;
        private SerializedProperty _valueChangedEventBusProperty;

        protected virtual EditorTypeHint typeHint => EditorTypeHint.None;
        
        protected virtual Func<TValue, TValue> GetDrawFieldOfTValueFunction(string label) => 
            v => (TValue)EditorGUIFunctions.DrawField(label, v, typeof(TValue), typeHint);

        private void OnEnable()
        {
            _target = (T)target;
            _drawValueFieldFunction = GetDrawFieldOfTValueFunction("Runtime Value");
            _defaultValueProperty = serializedObject.FindProperty("_defaultValue");
            _valueChangedEventBusProperty = serializedObject.FindProperty("_valueChangedEventBus");
        }

        public override bool RequiresConstantRepaint() => true;
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_defaultValueProperty);
            EditorGUILayout.PropertyField(_valueChangedEventBusProperty);
            bool editable = EditorGUIFunctions.DrawRuntimeEditToggle(ref _allowRuntimeEdit);
            using var disabledScope = new EditorGUI.DisabledScope(!editable);
            var newValue = _drawValueFieldFunction.Invoke(_target.value);
            if (!newValue.Equals(_target.value))
                _target.value = newValue;
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    
    [CustomEditor(typeof(BoolSO), true)]
    public class BoolSOEditor : ValueSOTEditor<BoolSO, bool>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Bool;
    }

    [CustomEditor(typeof(IntSO), true)]
    public class IntSOEditor : ValueSOTEditor<IntSO, int>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Int;
    }

    [CustomEditor(typeof(FloatSO), true)]
    public class FloatSOEditor : ValueSOTEditor<FloatSO, float>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Float;
    }

    [CustomEditor(typeof(StringSO), true)]
    public class StringSOEditor : ValueSOTEditor<StringSO, string>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.String;
    }

    [CustomEditor(typeof(ColorSO), true)]
    public class ColorSOEditor : ValueSOTEditor<ColorSO, Color>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Color;
    }

    [CustomEditor(typeof(CurveSO), true)]
    public class CurveSOEditor : ValueSOTEditor<CurveSO, AnimationCurve>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Curve;
    }

    [CustomEditor(typeof(GameObjectSO), true)]
    public class GameObjectSOEditor : ValueSOTEditor<GameObjectSO, GameObject>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.UnityObject;
    }

    [CustomEditor(typeof(GradientSO), true)]
    public class GradientSOEditor : ValueSOTEditor<GradientSO, Gradient>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Gradient;
    }

    [CustomEditor(typeof(TransformSO), true)]
    public class TransformSOEditor : ValueSOTEditor<TransformSO, Transform>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.UnityObject;
    }

    [CustomEditor(typeof(Vector2SO), true)]
    public class Vector2SOEditor : ValueSOTEditor<Vector2SO, Vector2>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Vector2;
    }

    [CustomEditor(typeof(Vector3SO), true)]
    public class Vector3SOEditor : ValueSOTEditor<Vector3SO, Vector3>
    {
        protected override EditorTypeHint typeHint => EditorTypeHint.Vector3;
    }
}