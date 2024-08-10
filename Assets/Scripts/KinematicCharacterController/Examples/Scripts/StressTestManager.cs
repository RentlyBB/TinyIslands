using UnityEngine;
using UnityEngine.UI;

namespace KinematicCharacterController.Examples {
    public class StressTestManager : MonoBehaviour {
        public Camera Camera;
        public LayerMask UIMask;

        public InputField CountField;
        public Image RenderOn;
        public Image SimOn;
        public Image InterpOn;
        public ExampleCharacterController CharacterPrefab;
        public ExampleAIController AIController;
        public int SpawnCount = 100;
        public float SpawnDistance = 2f;

        private void Start() {
            KinematicCharacterSystem.EnsureCreation();
            CountField.text = SpawnCount.ToString();
            UpdateOnImages();

            KinematicCharacterSystem.Settings.AutoSimulation = false;
            KinematicCharacterSystem.Settings.Interpolate = false;
        }

        private void Update() {
            KinematicCharacterSystem.Simulate(Time.deltaTime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);
        }

        private void UpdateOnImages() {
            RenderOn.enabled = Camera.cullingMask == -1;

            // SimOn.enabled = Physics.autoSimulation;

            SimOn.enabled = IsSimulationOn();
            InterpOn.enabled = KinematicCharacterSystem.Settings.Interpolate;
        }

        private bool IsSimulationOn() {
            var isSimOn = Physics.simulationMode == SimulationMode.FixedUpdate;

            return isSimOn;
        }

        public void SetSpawnCount(string count) {
            if (int.TryParse(count, out var result)) SpawnCount = result;
        }

        public void ToggleRendering() {
            if (Camera.cullingMask == -1)
                Camera.cullingMask = UIMask;
            else
                Camera.cullingMask = -1;
            UpdateOnImages();
        }

        public void TogglePhysicsSim() {
            //Physics.autoSimulation = !Physics.autoSimulation;

            if (Physics.simulationMode == SimulationMode.FixedUpdate)
                // Switch to manual simulation mode
                Physics.simulationMode = SimulationMode.Script;
            else
                // Switch back to automatic fixed update simulation mode
                Physics.simulationMode = SimulationMode.FixedUpdate;

            UpdateOnImages();
        }

        public void ToggleInterpolation() {
            KinematicCharacterSystem.Settings.Interpolate = !KinematicCharacterSystem.Settings.Interpolate;
            UpdateOnImages();
        }

        public void Spawn() {
            for (var i = 0; i < AIController.Characters.Count; i++) Destroy(AIController.Characters[i].gameObject);
            AIController.Characters.Clear();

            var charsPerRow = Mathf.CeilToInt(Mathf.Sqrt(SpawnCount));
            var firstPos = charsPerRow * SpawnDistance * 0.5f * -Vector3.one;
            firstPos.y = 0f;

            for (var i = 0; i < SpawnCount; i++) {
                var row = i / charsPerRow;
                var col = i % charsPerRow;
                var pos = firstPos + Vector3.right * row * SpawnDistance + Vector3.forward * col * SpawnDistance;

                var newChar = Instantiate(CharacterPrefab);
                newChar.Motor.SetPosition(pos);

                AIController.Characters.Add(newChar);
            }
        }
    }
}