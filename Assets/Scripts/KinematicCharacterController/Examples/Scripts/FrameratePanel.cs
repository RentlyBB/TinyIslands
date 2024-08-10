using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KinematicCharacterController.Examples {
    public class FrameratePanel : MonoBehaviour {
        public float PollingRate = 1f;
        public Text PhysicsRate;
        public Text PhysicsFPS;
        public Text AvgFPS;
        public Text AvgFPSMin;
        public Text AvgFPSMax;

        public string[] FramerateStrings = new string[0];

        private int _framesCount;
        private float _framesDeltaSum;

        private bool _isFixedUpdateThisFrame;
        private float _maxDeltaTimeForAvg = Mathf.NegativeInfinity;
        private float _minDeltaTimeForAvg = Mathf.Infinity;
        private int _physFramesCount;
        private float _physFramesDeltaSum;
        private float _timeOfLastPoll;
        private bool _wasFixedUpdateLastFrame;

        public Action<float> OnPhysicsFPSReady;

        private void Update() {
            // Regular frames
            _framesCount++;
            _framesDeltaSum += Time.deltaTime;

            // Max and min
            if (Time.deltaTime < _minDeltaTimeForAvg) _minDeltaTimeForAvg = Time.deltaTime;
            if (Time.deltaTime > _maxDeltaTimeForAvg) _maxDeltaTimeForAvg = Time.deltaTime;

            // Fixed frames
            if (_wasFixedUpdateLastFrame) {
                _wasFixedUpdateLastFrame = false;

                _physFramesCount++;
                _physFramesDeltaSum += Time.deltaTime;
            }

            if (_isFixedUpdateThisFrame) {
                _wasFixedUpdateLastFrame = true;
                _isFixedUpdateThisFrame = false;
            }

            // Polling timer
            var timeSinceLastPoll = Time.unscaledTime - _timeOfLastPoll;
            if (timeSinceLastPoll > PollingRate) {
                var physicsFPS = 1f / (_physFramesDeltaSum / _physFramesCount);

                AvgFPS.text = GetNumberString(Mathf.RoundToInt(1f / (_framesDeltaSum / _framesCount)));
                AvgFPSMin.text = GetNumberString(Mathf.RoundToInt(1f / _maxDeltaTimeForAvg));
                AvgFPSMax.text = GetNumberString(Mathf.RoundToInt(1f / _minDeltaTimeForAvg));
                PhysicsFPS.text = GetNumberString(Mathf.RoundToInt(physicsFPS));

                if (OnPhysicsFPSReady != null) OnPhysicsFPSReady(physicsFPS);

                _physFramesDeltaSum = 0;
                _physFramesCount = 0;
                _framesDeltaSum = 0f;
                _framesCount = 0;
                _minDeltaTimeForAvg = Mathf.Infinity;
                _maxDeltaTimeForAvg = Mathf.NegativeInfinity;

                _timeOfLastPoll = Time.unscaledTime;
            }

            PhysicsRate.text = GetNumberString(Mathf.RoundToInt(1f / Time.fixedDeltaTime));
        }

        private void FixedUpdate() {
            _isFixedUpdateThisFrame = true;
        }

        public string GetNumberString(int fps) {
            if (fps < FramerateStrings.Length - 1 && fps >= 0) return FramerateStrings[fps];
            return FramerateStrings[FramerateStrings.Length - 1];
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FrameratePanel))]
    public class FrameratePanelEditor : Editor {
        private const int MaxFPS = 999;

        private void OnEnable() {
            InitStringsArray();
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if (GUILayout.Button("Init strings array")) InitStringsArray();
        }

        private void InitStringsArray() {
            var fp = target as FrameratePanel;
            fp.FramerateStrings = new string[MaxFPS + 1];

            for (var i = 0; i < fp.FramerateStrings.Length; i++)
                if (i >= fp.FramerateStrings.Length - 1)
                    fp.FramerateStrings[i] = i + "+" + " (<" + (1000f / i).ToString("F") + "ms)";
                else
                    fp.FramerateStrings[i] = i + " (" + (1000f / i).ToString("F") + "ms)";
        }
    }
#endif
}