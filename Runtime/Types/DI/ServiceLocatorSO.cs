using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableFlow.Runtime.DI
{
    public sealed class ServiceLocatorSO : ScriptableObject, IServiceLocator
    {
        [NonSerialized] private Dictionary<Type, object> _services;
        
        private void OnEnable()
        {
            _services = new();
        }
        
        public ServiceLocatorType serviceLocatorType => ServiceLocatorType.Basic;

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
            return (T)_services[typeof(T)];
        }

        public bool ResolveSafe<T>(out T service, T noService = default) where T : class
        {
            service = noService;
            if (!_services.TryGetValue(typeof(T), out var foundService))
                return false;
            service = (T)foundService;
            return true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
                _services = null;
        }
#endif
    }
}