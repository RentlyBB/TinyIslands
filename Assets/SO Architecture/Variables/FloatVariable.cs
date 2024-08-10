using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [CreateAssetMenu(
        fileName = "FloatVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "float",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 3)]
    public class FloatVariable : BaseVariable<float, FloatEvent> {
        public override bool Clampable => true;

        protected override float ClampValue(float value) {
            if (value.CompareTo(MinClampValue) < 0) return MinClampValue;
            if (value.CompareTo(MaxClampValue) > 0) return MaxClampValue;
            return value;
        }
    }
}