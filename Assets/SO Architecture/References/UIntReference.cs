using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class UIntReference : BaseReference<uint, UIntVariable> {
        public UIntReference() { }
        public UIntReference(uint value) : base(value) { }
    }
}