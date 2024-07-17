using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class Vector3UnityEvent : UnityEvent<Vector3> {
    }
}