using System;
using UnityEngine;
using World.Enums;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "DiceEventHandler", menuName = "Game/DiceEventHandler", order = 0)]
    public class DiceEventHandlerSo : ScriptableObject {
        public event Action<DiceFaces> OnEventRaised;

        public void RaiseEvent(DiceFaces diceFace) {
            OnEventRaised?.Invoke(diceFace);
        }
    }
}