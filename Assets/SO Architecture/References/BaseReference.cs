using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class BaseReference<TBase, TVariable> : BaseReference where TVariable : BaseVariable<TBase> {
        [SerializeField]
        protected bool _useConstant;

        [SerializeField]
        protected TBase _constantValue;

        [SerializeField]
        protected TVariable _variable;

        public BaseReference() { }

        public BaseReference(TBase baseValue) {
            _useConstant = true;
            _constantValue = baseValue;
        }

        public TBase Value {
            get => _useConstant || _variable == null ? _constantValue : _variable.Value;
            set {
                if (!_useConstant && _variable != null) {
                    _variable.Value = value;
                } else {
                    _useConstant = true;
                    _constantValue = value;
                }
            }
        }

        public bool IsValueDefined => _useConstant || _variable != null;

        public BaseReference CreateCopy() {
            var copy = (BaseReference<TBase, TVariable>)Activator.CreateInstance(GetType());
            copy._useConstant = _useConstant;
            copy._constantValue = _constantValue;
            copy._variable = _variable;

            return copy;
        }

        public void AddListener(IGameEventListener listener) {
            if (_variable != null)
                _variable.AddListener(listener);
        }

        public void RemoveListener(IGameEventListener listener) {
            if (_variable != null)
                _variable.RemoveListener(listener);
        }

        public void AddListener(Action action) {
            if (_variable != null)
                _variable.AddListener(action);
        }

        public void RemoveListener(Action action) {
            if (_variable != null)
                _variable.RemoveListener(action);
        }

        public override string ToString() {
            return Value.ToString();
        }
    }

    //Can't get property drawer to work with generic arguments
    public abstract class BaseReference { }
}