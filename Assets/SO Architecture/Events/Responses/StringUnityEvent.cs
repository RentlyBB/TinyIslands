using System;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class StringUnityEvent : UnityEvent<string> { }
}