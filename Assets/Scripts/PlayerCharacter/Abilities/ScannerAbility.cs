using EditorScripts;
using EditorScripts.InvokeButton;
using UnityEngine;

namespace PlayerCharacter.Abilities {
    public class ScannerAbility : MonoBehaviour {
        // Maybe it should not be here, but object should be saved somewhere else where managers will have access to it.
        public GameObject scannedObject;

        public Vector3 offset;

        public void SaveScannedObject(GameObject obj) {
            Debug.Log("Scanned object: " + obj.name);
            scannedObject = obj;
        }


        //Placing object should be avaiable only on some placeces. Not everywhere.
        [InvokeButton]
        public void PlaceScannedObject() {
            if (scannedObject == null) return;

            Instantiate(scannedObject, transform.position + offset, Quaternion.identity, null);
        }
    }
}