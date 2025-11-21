using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace ScriptableFlow.Runtime.DI
{
    public sealed class LockedServiceLocatorSO : ScriptableObject, IServiceLocator
    {
        private enum State
        {
            Init = 0,
            Bind = 1,
            Resolve = 2
        }

        private Guid _key;
        private State _state;
        private Dictionary<Type, object> _services;

        private void OnEnable()
        {
            _state = State.Init;
            _services = new();
        }

        public void Init(Guid key)
        {
            EnsureInInitPhase();
            _key = key;
            _state = State.Bind;
        }

        public bool Unlock(Guid key)
        {
            EnsureInResolvePhase();
            if (key != _key)
                return false;

            _state = State.Bind;
            return true;
        }

        public bool Lock(Guid key)
        {
            EnsureInBindPhase();
            if (key != _key)
                return false;

            _state = State.Resolve;
            return true;
        }

        public ServiceLocatorType serviceLocatorType => ServiceLocatorType.Locked;

        public void Bind<T>(T service) where T : class
        {
            EnsureInResolvePhase();
            _services.Add(typeof(T), service);
        }

        public bool TryBind<T>(T service) where T : class
        {
            EnsureInResolvePhase();
            if (_services.ContainsKey(typeof(T)))
                return false;

            _services.Add(typeof(T), service);
            return true;
        }

        public void Rebind<T>(T service) where T : class
        {
            EnsureInResolvePhase();
            _services[typeof(T)] = service;
        }

        public bool UnbindInstance<T>(T service) where T : class
        {
            EnsureInResolvePhase();
            if (!_services.TryGetValue(typeof(T), out var foundService) || service != foundService)
                return false;

            return _services.Remove(typeof(T));
        }

        public bool Unbind<T>() where T : class
        {
            EnsureInResolvePhase();
            return _services.Remove(typeof(T));
        }

        public void UnbindAll()
        {
            EnsureInResolvePhase();
            _services.Clear();
        }

        public T Resolve<T>() where T : class
        {
            EnsureInBindPhase();
            if (!_services.TryGetValue(typeof(T), out var service))
                throw new Exception($"No service binding found for {typeof(T)}");

            return (T)service;
        }

        public bool ResolveSafe<T>(out T service, T noService = default) where T : class
        {
            EnsureInBindPhase();
            service = noService;
            if (!_services.TryGetValue(typeof(T), out var foundService))
                return false;

            service = (T)foundService;
            return true;
        }

        private void EnsureInInitPhase()
        {
            if (_state == State.Init)
                throw new ConstraintException($"ServiceLocator {this} fails Init state guarantees");
        }

        private void EnsureInBindPhase()
        {
            if (_state != State.Bind)
                throw new ConstraintException($"ServiceLocator {this} fails Bind state guarantees");
        }

        private void EnsureInResolvePhase()
        {
            if (_state != State.Resolve)
                throw new ConstraintException($"ServiceLocator {this} fails Resolve state guarantees");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
            {
                _state = State.Init;
                _services = null;
            }
        }
#endif
    }
}