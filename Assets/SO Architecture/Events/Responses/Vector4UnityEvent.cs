using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class Vector4UnityEvent : UnityEvent<Vector4> { }
}