using UnityEngine;
using Utils;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "PlayerManager", menuName = "/Game/PlayerManager", order = 2)]
    public class PlayerManager : SingletonScriptableObject<PlayerManager> {


        public int i;

        public string text;


    }
}