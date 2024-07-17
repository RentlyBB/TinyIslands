using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    public abstract class GameEventBase<T> : GameEventBase, IGameEvent<T>, IStackTraceObject {

        [SerializeField]
        protected T _debugValue;
        private readonly List<Action<T>> _typedActions = new List<Action<T>>();
        private readonly List<IGameEventListener<T>> _typedListeners = new List<IGameEventListener<T>>();

        public void Raise(T value) {
            AddStackTrace(value);

            for (int i = _typedListeners.Count - 1; i >= 0; i--)
                _typedListeners[i].OnEventRaised(value);

            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();

            for (int i = _typedActions.Count - 1; i >= 0; i--)
                _typedActions[i](value);

            for (int i = _actions.Count - 1; i >= 0; i--)
                _actions[i]();
        }
        public void AddListener(IGameEventListener<T> listener) {
            if (!_typedListeners.Contains(listener))
                _typedListeners.Add(listener);
        }
        public void RemoveListener(IGameEventListener<T> listener) {
            if (_typedListeners.Contains(listener))
                _typedListeners.Remove(listener);
        }
        public void AddListener(Action<T> action) {
            if (!_typedActions.Contains(action))
                _typedActions.Add(action);
        }
        public void RemoveListener(Action<T> action) {
            if (_typedActions.Contains(action))
                _typedActions.Remove(action);
        }
        public override string ToString() {
            return "GameEventBase<" + typeof(T) + ">";
        }
    }
    public abstract class GameEventBase : SOArchitectureBaseObject, IGameEvent, IStackTraceObject {
        protected readonly List<Action> _actions = new List<Action>();
        protected readonly List<IGameEventListener> _listeners = new List<IGameEventListener>();

        public void Raise() {
            AddStackTrace();

            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();

            for (int i = _actions.Count - 1; i >= 0; i--)
                _actions[i]();
        }
        public void AddListener(IGameEventListener listener) {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }
        public void RemoveListener(IGameEventListener listener) {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }
        public virtual void RemoveAll() {
            _listeners.RemoveRange(0, _listeners.Count);
            _actions.RemoveRange(0, _actions.Count);
        }

        public List<StackTraceEntry> StackTraces { get; } = new List<StackTraceEntry>();

        public void AddStackTrace() {
#if UNITY_EDITOR
            if (SOArchitecturePreferences.IsDebugEnabled)
                StackTraces.Insert(0, StackTraceEntry.Create());
#endif
        }
        public void AddStackTrace(object value) {
#if UNITY_EDITOR
            if (SOArchitecturePreferences.IsDebugEnabled)
                StackTraces.Insert(0, StackTraceEntry.Create(value));
#endif
        }
        public void AddListener(Action action) {
            if (!_actions.Contains(action))
                _actions.Add(action);
        }
        public void RemoveListener(Action action) {
            if (_actions.Contains(action))
                _actions.Remove(action);
        }
    }
}