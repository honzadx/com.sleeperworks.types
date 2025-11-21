using System;
using UnityEngine;

namespace ScriptableFlow.Runtime.Events
{
    public abstract class ResultEventBusSOT<T> : ScriptableObject, IRaiseEventT<T>, IListenEventT<T>
    {
        [SerializeField] private EventLogOptions _eventLogOptions;

        internal Action<T> @event;

        public void Raise(T value)
        {
            EventBusLogger.RaiseLog(this, value, _eventLogOptions);
            @event?.Invoke(value);
        }

        public void AddListener(Action<T> callback)
        {
            EventBusLogger.AddListenerLog(this, callback, _eventLogOptions);
            @event -= callback;
            @event += callback;
        }

        public void RemoveListener(Action<T> callback)
        {
            EventBusLogger.RemoveListenerLog(this, callback, _eventLogOptions);
            @event -= callback;
        }

        public void ClearListeners()
        {
            EventBusLogger.ClearListenersLog(this, _eventLogOptions);
            @event = null;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
                @event = null;
        }
#endif
    }
}