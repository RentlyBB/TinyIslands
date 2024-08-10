using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class Vector2UnityEvent : UnityEvent<Vector2> { }
}