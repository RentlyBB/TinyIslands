using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    [CreateAssetMenu(
        fileName = "ShortGameEvent.asset",
        menuName = SOArchitecture_Utility.ADVANCED_GAME_EVENT + "short",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 14)]
    public sealed class ShortGameEvent : GameEventBase<short> { }
}