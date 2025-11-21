using System.Collections.Generic;
using UnityEngine;
using ScriptableFlow.Runtime.Events;

namespace ScriptableFlow.Runtime
{
    public class ListSOT<T> : ScriptableObject
    {
        [SerializeField] private List<T> _defaultList;
        [SerializeField] private SignalEventBusSO _onListUpdatedEventBus;

        private List<T> _runtimeList;

        public IReadOnlyList<T> runtimeList => _runtimeList;

        private void OnEnable()
        {
            _runtimeList = _defaultList;
        }

        public int GetItemCount() => _runtimeList.Count;

        public void AddItem(T item)
        {
            _runtimeList.Add(item);
            _onListUpdatedEventBus?.Raise();
        }

        public void RemoveItem(T item)
        {
            _runtimeList.Remove(item);
            _onListUpdatedEventBus?.Raise();
        }

        public void ClearItems()
        {
            _runtimeList.Clear();
            _onListUpdatedEventBus?.Raise();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode &&
                !UnityEditor.EditorApplication.isPlaying)
            {
                _runtimeList = _defaultList;
            }
        }
#endif
    }
}