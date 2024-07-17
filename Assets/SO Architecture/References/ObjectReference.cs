using System;
using Object = UnityEngine.Object;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class ObjectReference : BaseReference<Object, ObjectVariable> {
        public ObjectReference() { }
        public ObjectReference(Object value) : base(value) { }
    }
}