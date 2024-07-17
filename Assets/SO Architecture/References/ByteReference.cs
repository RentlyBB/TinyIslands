using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class ByteReference : BaseReference<byte, ByteVariable> {
        public ByteReference() { }
        public ByteReference(byte value) : base(value) { }
    }
}