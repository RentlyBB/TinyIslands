using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class CharReference : BaseReference<char, CharVariable> {
        public CharReference() { }
        public CharReference(char value) : base(value) { }
    }
}