using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class UShortReference : BaseReference<ushort, UShortVariable> {
        public UShortReference() { }
        public UShortReference(ushort value) : base(value) { }
    }
}