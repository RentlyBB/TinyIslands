using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class IntReference : BaseReference<int, IntVariable> {
        public IntReference() { }
        public IntReference(int value) : base(value) { }
    }
}