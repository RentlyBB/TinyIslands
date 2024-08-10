using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class UIntEvent : UnityEvent<uint> { }

    [CreateAssetMenu(
        fileName = "UnsignedIntVariable.asset",
        menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "uint",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 16)]
    public class UIntVariable : BaseVariable<uint, UIntEvent> {
        public override bool Clampable => true;

        protected override uint ClampValue(uint value) {
            if (value.CompareTo(MinClampValue) < 0) return MinClampValue;
            if (value.CompareTo(MaxClampValue) > 0) return MaxClampValue;
            return value;
        }
    }
}