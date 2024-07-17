using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class Vector2Reference : BaseReference<Vector2, Vector2Variable> {
        public Vector2Reference() { }
        public Vector2Reference(Vector2 value) : base(value) { }
    }
}