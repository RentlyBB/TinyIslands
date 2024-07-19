using EditorScripts;
using UnityEngine;
using UnityEngine.Events;
using World.Interfaces;

namespace World {
    public class DiceSocketBehaviour : MonoBehaviour, IPuzzle {

        public Transform dice;
        public Transform diceSocketPosition;

        public DiceFaces targetFace;
        public bool autoCheckPuzzle = true;

        public UnityEvent onSolve;

        private DiceBehaviour _diceBehaviour;

        private bool _isSolved;


        private void Start() {
            dice.TryGetComponent(out _diceBehaviour);
        }

        private void Update() {
            if (autoCheckPuzzle && !_isSolved) {
                _diceBehaviour.lockAnim = true;
                CheckAndResolve();
            } else {
                _diceBehaviour.lockAnim = false;
            }
        }

        public void CheckPuzzle() {
            if (!_diceBehaviour.rotationCompleted)
                return;

            if (targetFace != _diceBehaviour.currentSide) {
                _isSolved = false;
            } else {
                _isSolved = true;
            }
        }
        public void WrongSolution() {
            _diceBehaviour.Shake();
        }

        public void ResolvePuzzle() {
            if (!_isSolved) {
                _diceBehaviour.Shake();
            } else {
                _diceBehaviour.SetTargetPosition(diceSocketPosition.position);
                _diceBehaviour.locked = true;
                onSolve?.Invoke();
            }
        }

        public bool IsSolved() {
            return _isSolved;
        }

        [InvokeButton]
        public void CheckAndResolve() {
            CheckPuzzle();
            ResolvePuzzle();
        }
    }
}