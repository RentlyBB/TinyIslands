using UnityEngine;

namespace KinematicCharacterController.Examples {
    public class PrefabLauncher : MonoBehaviour {
        public Rigidbody ToLaunch;
        public float Force;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                var inst = Instantiate(ToLaunch, transform.position, transform.rotation);
                inst.AddForce(transform.forward * Force, ForceMode.VelocityChange);
                Destroy(inst.gameObject, 8f);
            }
        }
    }
}