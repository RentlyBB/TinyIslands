using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class ShortEvent : UnityEvent<short> { }

    [CreateAssetMenu(
        fileName = "ShortVariable.asset",
        menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "short",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 14)]
    public class ShortVariable : BaseVariable<short, ShortEvent> {
        public override bool Clampable => true;

        protected override short ClampValue(short value) {
            if (value.CompareTo(MinClampValue) < 0) return MinClampValue;
            if (value.CompareTo(MaxClampValue) > 0) return MaxClampValue;
            return value;
        }
    }
}