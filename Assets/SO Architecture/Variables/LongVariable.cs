using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class LongEvent : UnityEvent<long> { }

    [CreateAssetMenu(
        fileName = "LongVariable.asset",
        menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "long",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 9)]
    public class LongVariable : BaseVariable<long, LongEvent> {
        public override bool Clampable => true;

        protected override long ClampValue(long value) {
            if (value.CompareTo(MinClampValue) < 0) return MinClampValue;
            if (value.CompareTo(MaxClampValue) > 0) return MaxClampValue;
            return value;
        }
    }
}