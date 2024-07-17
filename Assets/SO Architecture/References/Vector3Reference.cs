using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class Vector3Reference : BaseReference<Vector3, Vector3Variable> {
        public Vector3Reference() { }
        public Vector3Reference(Vector3 value) : base(value) { }
    }
}