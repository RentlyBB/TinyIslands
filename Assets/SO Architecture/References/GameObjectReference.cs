using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class GameObjectReference : BaseReference<GameObject, GameObjectVariable> {
        public GameObjectReference() { }
        public GameObjectReference(GameObject value) : base(value) { }
    }
}