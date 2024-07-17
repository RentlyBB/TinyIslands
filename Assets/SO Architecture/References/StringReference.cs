using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class StringReference : BaseReference<string, StringVariable> {
        public StringReference() { }
        public StringReference(string value) : base(value) { }
    }
}