using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class AnimationCurveReference : BaseReference<AnimationCurve, AnimationCurveVariable> {
        public AnimationCurveReference() { }
        public AnimationCurveReference(AnimationCurve value) : base(value) { }
    }
}