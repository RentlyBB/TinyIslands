using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class IntEvent : UnityEvent<int> { }

    [CreateAssetMenu(
        fileName = "IntVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "int",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 4)]
    public class IntVariable : BaseVariable<int, IntEvent> {
        public override bool Clampable => true;

        protected override int ClampValue(int value) {
            if (value.CompareTo(MinClampValue) < 0) return MinClampValue;
            if (value.CompareTo(MaxClampValue) > 0) return MaxClampValue;
            return value;
        }
    }
}