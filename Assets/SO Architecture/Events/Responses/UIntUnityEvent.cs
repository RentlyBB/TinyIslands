using System;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class UIntUnityEvent : UnityEvent<uint> {
    }
}