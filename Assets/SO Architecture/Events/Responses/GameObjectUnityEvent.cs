using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class GameObjectUnityEvent : UnityEvent<GameObject> { }
}