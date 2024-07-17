using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class LayerMaskReference : BaseReference<LayerMask, LayerMaskVariable> {
        public LayerMaskReference() { }
        public LayerMaskReference(LayerMask value) : base(value) { }
    }
}