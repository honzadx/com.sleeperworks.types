using System;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    [Serializable]
    public class ValueReferenceT<T>
    {
        [SerializeField] protected internal bool _useScriptableReference;
        [SerializeField] protected internal T _value;
        [SerializeField] protected internal ValueSOT<T> _scriptableValue;
        
        public T value {
            get => _useScriptableReference ? _scriptableValue.value : _value;
            set {
                if (_useScriptableReference)
                {
                    _scriptableValue.value = value;
                }
                else
                {
                    _value = value;
                }
            }
        }
    }
    
    [Serializable]
    public class BoolReference : ValueReferenceT<bool> { }
    
    [Serializable]
    public class FloatReference : ValueReferenceT<float> { }
    
    [Serializable]
    public class IntReference :  ValueReferenceT<int> { }
    
    [Serializable]
    public class StringReference : ValueReferenceT<string> { }
    
    [Serializable]
    public class ColorReference : ValueReferenceT<Color> { }
    
    [Serializable]
    public class CurveReference : ValueReferenceT<AnimationCurve> { }
    
    [Serializable]
    public class GradientReference : ValueReferenceT<Gradient> { }
    
    [Serializable]
    public class Vector2Reference : ValueReferenceT<Vector2> { }
    
    [Serializable]
    public class Vector2IntReference : ValueReferenceT<Vector2Int> { }
    
    [Serializable]
    public class Vector3Reference : ValueReferenceT<Vector3> { }
    
    [Serializable]
    public class Vector3IntReference : ValueReferenceT<Vector3Int> { }
    
    [Serializable]
    public class Vector4Reference : ValueReferenceT<Vector4> { }
}