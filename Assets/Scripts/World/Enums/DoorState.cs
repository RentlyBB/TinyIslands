using System.Collections.Generic;

namespace World.Enums {
    
    
    public struct DoorUtils {
        public enum DoorState {
            Opened,
            Closed
        }
      
        public static Dictionary<DoorState, bool> DoorStateDictionary = new Dictionary<DoorState, bool> {
            { DoorState.Opened, true },
            { DoorState.Closed, false },
        };

        public static bool EvaluateDoorState(DoorState doorState) {
            return DoorStateDictionary[doorState];
        }
        
    }
}