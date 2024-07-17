using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class Vector4Reference : BaseReference<Vector4, Vector4Variable> {
        public Vector4Reference() { }
        public Vector4Reference(Vector4 value) : base(value) { }
    }
}