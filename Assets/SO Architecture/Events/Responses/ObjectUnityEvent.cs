using System;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class ObjectUnityEvent : UnityEvent<Object> { }
}