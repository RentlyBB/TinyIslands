using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class BoolReference : BaseReference<bool, BoolVariable> {
        public BoolReference() { }
        public BoolReference(bool value) : base(value) { }
    }
}