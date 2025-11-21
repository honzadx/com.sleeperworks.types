using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableFlow.Runtime.DI
{
    public sealed class ScopedServiceLocatorSO : ScriptableObject, IServiceLocator
    {
        [SerializeField] private ScopedServiceLocatorSO _parent;
        [NonSerialized] private Dictionary<Type, object> _services;
        
        private void OnEnable()
        {
            _services = new();
        }
        
        public ServiceLocatorType serviceLocatorType => ServiceLocatorType.Scoped;

        public void Bind<T>(T service) where T : class
        {
            _services.Add(typeof(T), service);
        }

        public bool TryBind<T>(T service) where T : class
        {
            if (_services.ContainsKey(typeof(T)))
                return false;
            _services.Add(typeof(T), service);
            return true;
        }

        public void Rebind<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        public bool UnbindInstance<T>(T service) where T : class
        {
            if (!_services.TryGetValue(typeof(T), out var foundService) || service != foundService)
                return false;
            return _services.Remove(typeof(T));
        }

        public bool Unbind<T>() where T : class
        {
            return _services.Remove(typeof(T));
        }

        public void UnbindAll()
        {
            _services.Clear();
        }

        public T Resolve<T>() where T : class
        {
            if (!_services.TryGetValue(typeof(T), out var service))
            {
                return _parent?.Resolve<T>() ?? throw new Exception($"No service binding found for {typeof(T)}");
            }

            return (T)service;
        }

        public bool ResolveSafe<T>(out T service, T noService = default) where T : class
        {
            service = noService;
            if (!_services.TryGetValue(typeof(T), out var foundService))
                return _parent.ResolveSafe(out service);
            service = (T)foundService;
            return true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
                _services = null;

            ValidateNoCyclicalReferences();

            void ValidateNoCyclicalReferences()
            {
                HashSet<ScopedServiceLocatorSO> visited = new();
                visited.Add(this);
                int depth = 0;
                var current = this;
                while (current._parent != null)
                {
                    depth++;
                    if (!visited.Add(current._parent))
                    {
                        current._parent = null;
                        EditorUtility.SetDirty(current);
                        Debug.LogWarning($"Cyclical parent reference detected in {this} at depth {depth}, " +
                                         "clearing parent to avoid infinite loops.");
                        return;
                    }

                    current = current._parent;
                }
            }
        }
#endif
    }
}