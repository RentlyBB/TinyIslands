using System.Collections.Generic;
using UnityEngine;
using Utils;
using World.Enums;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "PlayerManager", menuName = "Game/Managers/PlayerManager", order = 2)]
    public class PlayerManager : SingletonScriptableObject<PlayerManager> {

        [Header("Player's unlocked power core colors")]
        public List<PowerCoreColors> unlockColors;

        public void UnlockNewColor(PowerCoreColors unlockPowerCoreColor) {
            if(unlockColors.Contains(unlockPowerCoreColor)) return;
            
            unlockColors.Add(unlockPowerCoreColor);
        }
    }
}