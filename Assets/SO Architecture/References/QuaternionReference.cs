using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class QuaternionReference : BaseReference<Quaternion, QuaternionVariable> {
        public QuaternionReference() { }
        public QuaternionReference(Quaternion value) : base(value) { }
    }
}