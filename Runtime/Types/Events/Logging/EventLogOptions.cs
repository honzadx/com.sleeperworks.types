using System;

namespace ScriptableFlow.Runtime.Events
{
        [Flags]
        public enum EventLogOptions
        {
                None = 0,
                RaiseValue = 1 << 0,
                RaiseStackTrace = 1 << 1,
                RaiseListeners = 1 << 2,
                AddListener = 1 << 3,
                RemoveListener = 1 << 4,
                ClearListeners = 1 << 5,
        }
}