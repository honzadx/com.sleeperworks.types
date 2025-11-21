using System;
using UnityEngine;

namespace ScriptableFlow.Runtime.Events
{
    internal static class EventBusLogger
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void RaiseLog<T>(ResultEventBusSOT<T> resultEvent, T value, EventLogOptions logOption)
        {
            var shouldRaise = (int)logOption & ((int)EventLogOptions.RaiseValue |
                                                (int)EventLogOptions.RaiseStackTrace |
                                                (int)EventLogOptions.RaiseListeners);
            if (shouldRaise == 0)
                return;

            string log = $"[Event:{resultEvent}] Raised()\n\n";
            if (logOption.HasFlag(EventLogOptions.RaiseValue))
            {
                log += $"Value: {value}\n\n";
            }
            if (logOption.HasFlag(EventLogOptions.RaiseListeners))
            {
                log += "Listeners:\n";
                foreach (var callback in resultEvent.@event.GetInvocationList())
                {
                    log += $"{callback.Method.Name}: {callback.Target}\n";
                }
                log += "\n";
            }
            if (logOption.HasFlag(EventLogOptions.RaiseStackTrace))
            {
                log += $"Stack trace: {Environment.StackTrace}";
            }

            Debug.Log(log);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void RaiseLog(SignalEventBusSO signalEvent, EventLogOptions logOption)
        {
            var shouldRaise = (int)logOption & ((int)EventLogOptions.RaiseStackTrace |
                                                (int)EventLogOptions.RaiseListeners);
            if (shouldRaise == 0)
                return;

            string log = $"[Event:{signalEvent}] Raised()\n";
            if (logOption.HasFlag(EventLogOptions.RaiseStackTrace))
            {
                log += $"Stack trace: {Environment.StackTrace}";
            }

            if (logOption.HasFlag(EventLogOptions.RaiseListeners))
            {
                log += "Listeners:\n";
                foreach (var callback in signalEvent.@event.GetInvocationList())
                {
                    log += $"{callback.Method.Name}: {callback.Target}\n";
                }
            }

            Debug.Log(log);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void AddListenerLog<T>(ResultEventBusSOT<T> resultEvent, Action<T> callback,
            EventLogOptions logOptions)
        {
            if (!logOptions.HasFlag(EventLogOptions.AddListener))
                return;
            Debug.Log($"[Event:{resultEvent}] AddListener()\n{callback.Method.Name}: {callback.Target}");
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void AddListenerLog(SignalEventBusSO signalEvent, Action callback, EventLogOptions logOptions)
        {
            if (!logOptions.HasFlag(EventLogOptions.AddListener))
                return;
            Debug.Log($"[Event:{signalEvent}] AddListener()\n{callback.Method.Name}: {callback.Target}");
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void RemoveListenerLog<T>(ResultEventBusSOT<T> resultEvent, Action<T> callback,
            EventLogOptions logOption)
        {
            if (!logOption.HasFlag(EventLogOptions.RemoveListener))
                return;
            Debug.Log($"[Event:{resultEvent}] RemoveListener()\n{callback.Method.Name}: {callback.Target}");
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void RemoveListenerLog(SignalEventBusSO signalEvent, Action callback, EventLogOptions logOption)
        {
            if (!logOption.HasFlag(EventLogOptions.RemoveListener))
                return;
            Debug.Log($"[Event:{signalEvent}] RemoveListener()\n{callback.Method.Name}: {callback.Target}");
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void ClearListenersLog<T>(ResultEventBusSOT<T> resultEvent, EventLogOptions logOption)
        {
            if (!logOption.HasFlag(EventLogOptions.ClearListeners))
                return;
            Debug.Log($"[Event:{resultEvent}] ClearListeners()");
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void ClearListenersLog(SignalEventBusSO signalEvent, EventLogOptions logOption)
        {
            if (!logOption.HasFlag(EventLogOptions.ClearListeners))
                return;
            Debug.Log($"[Event:{signalEvent}] ClearListeners()");
        }
    }
}