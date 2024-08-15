using UnityEngine;
using World.Enums;

namespace World.Structs {
    
    public struct PowerCoreUtils {
        
        static public Color32 c_yellow {
            get {
                return new Color32(255, 255, 0, 255);
            }
        }
        
        static public Color32 c_red {
            get {
                return new Color32(255, 0, 0, 255);
            }
        }
        
        static public Color32 c_cyan {
            get {
                return new Color32(0, 255, 255, 255);
            }
        }
        
        static public Color32 c_green {
            get {
                return new Color32(0, 255, 0, 255);
            }
        }

        static public Color32 c_blue {
            get {
                return new Color32(0, 0, 255, 255);
            }
        }

        static public Color32 c_pink {
            get {
                return new Color32(255, 0, 255, 255);
            }
        }
        
        public static Color32 GetColor(PowerCoreColors colorType) {
            switch (colorType) {
                case PowerCoreColors.Yellow:
                    return c_yellow;
                case PowerCoreColors.Red:
                    return c_red;
                case PowerCoreColors.Cyan:
                    return c_cyan;
                case PowerCoreColors.Green:
                    return c_green;
                case PowerCoreColors.Blue:
                    return c_blue;
                case PowerCoreColors.Pink:
                    return c_pink;
            }
            return Color.black;
        }
    }
}