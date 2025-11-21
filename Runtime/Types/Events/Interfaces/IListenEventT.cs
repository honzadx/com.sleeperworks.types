using System;

namespace ScriptableFlow.Runtime.Events
{
    public interface IListenEventT<T>
    {
        public void AddListener(Action<T> callback);
        public void RemoveListener(Action<T> callback);
        public void ClearListeners();
    }
}