using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class ColorReference : BaseReference<Color, ColorVariable> {
        public ColorReference() { }
        public ColorReference(Color value) : base(value) { }
    }
}