using System;
using UnityEngine;
using World.Enums;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "PowerCoreEvent", menuName = "Game/PowerCoreEvent", order = 0)]
    public class PowerCoreEventSo : ScriptableObject {
        public event Action<PowerCoreColors> OnEventRaised;

        public void RaiseEvent(PowerCoreColors color) {
            OnEventRaised?.Invoke(color);
        }
    }
}