using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    [CreateAssetMenu(
        fileName = "UnsignedLongGameEvent.asset",
        menuName = SOArchitecture_Utility.ADVANCED_GAME_EVENT + "ulong",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 17)]
    public sealed class ULongGameEvent : GameEventBase<ulong> {
    }
}