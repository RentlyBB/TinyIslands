using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class SByteReference : BaseReference<sbyte, SByteVariable> {
        public SByteReference() { }
        public SByteReference(sbyte value) : base(value) { }
    }
}