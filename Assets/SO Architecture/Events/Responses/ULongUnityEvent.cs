using System;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class ULongUnityEvent : UnityEvent<ulong> { }
}