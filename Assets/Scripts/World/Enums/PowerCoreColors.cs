using System.Collections.Generic;
using UnityEngine;

namespace World.Enums {
    public enum PowerCoreColors {
        Yellow,
        Red,
        Cyan,
        Green,
        Blue,
        Pink,
        Any,
        None
    }
    
    public struct PowerCoreUtils {

        public static Dictionary<PowerCoreColors, Color32> PowerColorDictionary = new Dictionary<PowerCoreColors, Color32> {
            { PowerCoreColors.Yellow, new Color32(255, 255, 0, 255) },
            { PowerCoreColors.Red, new Color32(255, 0, 0, 255) },
            { PowerCoreColors.Cyan, new Color32(0, 255, 255, 255) },
            { PowerCoreColors.Green, new Color32(0, 255, 0, 255) },
            { PowerCoreColors.Blue, new Color32(0, 0, 255, 255) },
            { PowerCoreColors.Pink, new Color32(255, 0, 255, 255) }, 
            { PowerCoreColors.Any, new Color32(123, 123, 123, 255)},
            { PowerCoreColors.None, new Color32(255, 255, 255, 255)},
        };

        public static Color32 GetColor(PowerCoreColors powerColor) {
            return PowerColorDictionary[powerColor];
        }
    }
}