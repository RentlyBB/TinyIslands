using EditorScripts;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace World {
    public class Scannable : MonoBehaviour {
        
        
        [Header("Broadcasting events")]
        [SerializeField]
        private GameObjectGameEvent onScanObjectEvent = default(GameObjectGameEvent);
        
        //Broadcast game event with game object in it. 
        //Player new script called ScannerBehaviour will be listening to this event
        // and save the scannable object in ScannerBehaviour script


        [InvokeButton]
        public void Scanning() {
            onScanObjectEvent.Raise(gameObject);
        }
    }
}