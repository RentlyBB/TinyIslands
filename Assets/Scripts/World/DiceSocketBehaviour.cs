using System;
using EditorScripts;
using UnityEngine;
using UnityEngine.Events;

namespace World {
    public class DiceSocketBehaviour : MonoBehaviour {

        public Transform dice;
        public Transform diceSocketPosition;

        public DiceFaces targetFace;
        public bool autoCheckTarget = true;

        public UnityEvent onSolve;
        
        private DiceBehaviour _diceBehaviour;

        private bool _isSolved = false;
        

        private void Start() {
            dice.TryGetComponent(out _diceBehaviour);
        }

        private void Update() {
            if(autoCheckTarget)
                TargetCheck();
        }
        
        [InvokeButton]
        public void TargetCheck() {
            if (targetFace != _diceBehaviour.currentSide || !_diceBehaviour.rotationCompleted || _isSolved )
                return;

            _diceBehaviour.SetTargetPosition(diceSocketPosition.position);
            _diceBehaviour.locked = true;
            _isSolved = true;
            onSolve?.Invoke();
        }
    }
}