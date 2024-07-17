using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class DoubleReference : BaseReference<double, DoubleVariable> {
        public DoubleReference() { }
        public DoubleReference(double value) : base(value) { }
    }
}