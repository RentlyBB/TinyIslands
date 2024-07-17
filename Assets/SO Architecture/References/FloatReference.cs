using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class FloatReference : BaseReference<float, FloatVariable> {
        public FloatReference() { }
        public FloatReference(float value) : base(value) { }
    }
}