using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    [CreateAssetMenu(
        fileName = "UnsignedShortGameEvent.asset",
        menuName = SOArchitecture_Utility.ADVANCED_GAME_EVENT + "ushort",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 18)]
    public sealed class UShortGameEvent : GameEventBase<ushort> {
    }
}