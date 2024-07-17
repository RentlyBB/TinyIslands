using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class LongReference : BaseReference<long, LongVariable> {
        public LongReference() { }
        public LongReference(long value) : base(value) { }
    }
}