using UnityEngine;
using ScriptableFlow.Runtime.Events;

namespace ScriptableFlow.Runtime
{
    public abstract class ValueSOT<T> : ScriptableObject
    {
        [SerializeField] protected T _defaultValue;
        [SerializeReference] protected ResultEventBusSOT<T> _valueChangedEventBus;

        private T _value;

        public T value
        {
            get => _value;
            set
            {
                _value = value;
                _valueChangedEventBus?.Raise(value);
            }
        }

        private void OnEnable()
        {
            value = _defaultValue;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode &&
                !UnityEditor.EditorApplication.isPlaying)
            {
                value = _defaultValue;
            }
        }
#endif
    }
}