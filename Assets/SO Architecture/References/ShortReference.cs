using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class ShortReference : BaseReference<short, ShortVariable> {
        public ShortReference() { }
        public ShortReference(short value) : base(value) { }
    }
}