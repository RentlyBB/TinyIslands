using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "DiceEventHandler", menuName = "Game/DiceEventHandler", order = 0)]
    public class DiceEventHandlerSo : ScriptableObject {

        
        public event Action<String> OnEventRaised;

        public void RaiseEvent(String text) {
            OnEventRaised?.Invoke(text);
        }


    }
}