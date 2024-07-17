using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class Color32Reference : BaseReference<Color32, Color32Variable> {
        public Color32Reference() { }
        public Color32Reference(Color32 value) : base(value) { }
    }
}