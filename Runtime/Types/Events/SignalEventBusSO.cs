using System;
using UnityEngine;

namespace ScriptableFlow.Runtime.Events
{
    public class SignalEventBusSO : ScriptableObject, IRaiseEvent, IListenEvent
    {
        [SerializeField] private EventLogOptions _eventLogOptions;

        internal Action @event;

        public void Raise()
        {
            EventBusLogger.RaiseLog(this, _eventLogOptions);
            @event?.Invoke();
        }

        public void AddListener(Action callback)
        {
            EventBusLogger.AddListenerLog(this, callback, _eventLogOptions);
            @event -= callback;
            @event += callback;
        }

        public void RemoveListener(Action callback)
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