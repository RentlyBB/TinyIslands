using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class ULongEvent : UnityEvent<ulong> { }

    [CreateAssetMenu(
        fileName = "UnsignedLongVariable.asset",
        menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "ulong",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 17)]
    public class ULongVariable : BaseVariable<ulong, ULongEvent> {
        public override bool Clampable => true;

        protected override ulong ClampValue(ulong value) {
            if (value.CompareTo(MinClampValue) < 0) return MinClampValue;
            if (value.CompareTo(MaxClampValue) > 0) return MaxClampValue;
            return value;
        }
    }
}