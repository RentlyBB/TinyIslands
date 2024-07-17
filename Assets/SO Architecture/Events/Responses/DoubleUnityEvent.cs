using System;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class DoubleUnityEvent : UnityEvent<double> {
    }
}