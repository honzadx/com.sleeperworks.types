using AmeWorks.ScriptableFlow.Editor.Helpers;
using AmeWorks.ScriptableFlow.Runtime.Types;
using UnityEditor;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Editor.Inspectors
{
    public class ValueSOTInspector<TSO, TValue> : UnityEditor.Editor where TSO : ValueSOT<TValue>
    {
        private TSO _target;
        private SerializedProperty _guidProperty;
        private SerializedProperty _defaultValueProperty;
        private bool _editable;

        private void OnEnable()
        {
            _target = (TSO)target;
            _guidProperty = serializedObject.FindProperty("_guid");
            _defaultValueProperty =  serializedObject.FindProperty("_defaultValue");
        }

        public override void OnInspectorGUI()
        {
            EditorGUIFunctions.DrawMonoScript(_target);
            serializedObject.Update();
            EditorGUILayout.PropertyField(_guidProperty);
            EditorGUILayout.PropertyField(_defaultValueProperty);
            if (EditorApplication.isPlaying)
            {
                _editable = EditorGUILayout.Toggle("Edit Value", _editable);
                using (new EditorGUI.DisabledScope(!_editable))
                {
                    _target.value = RuntimeFieldDrawer<TValue>.DrawField("Runtime Value", _target.value);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(BoolSO), true)]
    public class BoolSOInspector : ValueSOTInspector<BoolSO, bool> { }
    
    [CustomEditor(typeof(FloatSO), true)]
    public class FloatSOInspector : ValueSOTInspector<FloatSO, float> { }
    
    [CustomEditor(typeof(IntSO), true)]
    public class IntSOInspector : ValueSOTInspector<IntSO, int> { }
    
    [CustomEditor(typeof(StringSO), true)]
    public class StringSOInspector : ValueSOTInspector<StringSO, string> { }
    
    [CustomEditor(typeof(ColorSO), true)]
    public class ColorSOInspector : ValueSOTInspector<ColorSO, Color> { }
    
    [CustomEditor(typeof(CurveSO), true)]
    public class CurveSOInspector : ValueSOTInspector<CurveSO, AnimationCurve> { }
    
    [CustomEditor(typeof(GradientSO), true)]
    public class GradientSOInspector : ValueSOTInspector<GradientSO, Gradient> { }
    
    [CustomEditor(typeof(Vector2SO), true)]
    public class Vector2SOInspector : ValueSOTInspector<Vector2SO, Vector2> { }
    
    [CustomEditor(typeof(Vector2IntSO), true)]
    public class Vector2IntSOInspector : ValueSOTInspector<Vector2IntSO, Vector2Int> { }
    
    [CustomEditor(typeof(Vector3SO), true)]
    public class Vector3SOInspector : ValueSOTInspector<Vector3SO, Vector3> { }
    
    [CustomEditor(typeof(Vector3IntSO), true)]
    public class Vector3IntSOInspector : ValueSOTInspector<Vector3IntSO, Vector3Int> { }
    
    [CustomEditor(typeof(Vector4SO), true)]
    public class Vector4SOInspector : ValueSOTInspector<Vector4SO, Vector4> { }
}