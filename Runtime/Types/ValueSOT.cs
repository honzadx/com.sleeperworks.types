using System;
using AmeWorks.ScriptableFlow.Runtime.Attributes;
using UnityEngine;

namespace AmeWorks.ScriptableFlow.Runtime.Types
{
    [CreateAsset(true)]
    public abstract class ValueSOT<T> : PersistentIdSO
    {
        [SerializeField] internal T _defaultValue;

        [NonSerialized] internal T _runtimeValue;
        
        public T defaultValue => _defaultValue;

        public T value
        {
            get => _runtimeValue;
            set => _runtimeValue = value;
        } 

        private void OnEnable()
        {
            ResetDefault();
        }

        public virtual void ResetDefault()
        {
            _runtimeValue = _defaultValue;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
            {
                ResetDefault();
            }
        }
#endif
    }
}