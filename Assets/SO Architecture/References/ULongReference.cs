using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class ULongReference : BaseReference<ulong, ULongVariable> {
        public ULongReference() { }
        public ULongReference(ulong value) : base(value) { }
    }
}