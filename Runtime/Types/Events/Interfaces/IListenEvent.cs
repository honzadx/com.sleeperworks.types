using System;

namespace ScriptableFlow.Runtime.Events
{
    public interface IListenEvent
    {
        public void AddListener(Action callback);
        public void RemoveListener(Action callback);
        public void ClearListeners();
    }
}