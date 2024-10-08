﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ScriptableObjectArchitecture {
    [Serializable]
    [CreateAssetMenu(
        fileName = "ObjectGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "Object",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 1)]
    public class ObjectGameEvent : GameEventBase<Object> { }
}