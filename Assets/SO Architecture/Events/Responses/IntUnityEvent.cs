using System;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class IntUnityEvent : UnityEvent<int> { }
}