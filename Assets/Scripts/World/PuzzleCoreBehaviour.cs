using System.Collections.Generic;
using System.Linq;
using EditorScripts;
using UnityEngine;
using UnityEngine.Events;
using World.Interfaces;

namespace World {
    public class PuzzleCoreBehaviour : MonoBehaviour {

        [TextArea] public string description;
        [Space]
        public bool autoResolve;
        [Space]
        public List<GameObject> puzzlesObj = new List<GameObject>();

        public UnityEvent onResolve;
        private readonly List<IPuzzle> _iPuzzles = new List<IPuzzle>();

        private readonly UnityEvent _eCheckPuzzle = new UnityEvent();
        private readonly UnityEvent _eResolvePuzzle = new UnityEvent();
        private readonly UnityEvent _eWrongSolutionPuzzle = new UnityEvent();


        private void Start() {
            if (puzzlesObj.Count != 0) {
                foreach (IPuzzle puz in puzzlesObj.Select(puzzle => puzzle.GetComponent<IPuzzle>())) {
                    _iPuzzles.Add(puz);
                    _eCheckPuzzle.AddListener(puz.CheckPuzzle);
                    _eResolvePuzzle.AddListener(puz.ResolvePuzzle);
                    _eWrongSolutionPuzzle.AddListener(puz.WrongSolution);
                }
            }
        }

        private void Update() {
            if (autoResolve) {
                ResolveAllPuzzles();
            }
        }

        [InvokeButton]
        public void ResolveAllPuzzles() {
            if (puzzlesObj.Count == 0) return;

            _eCheckPuzzle?.Invoke();

            // If one puzzle is wrong, return it
            foreach (IPuzzle puz in _iPuzzles) {
                if (!puz.IsSolved()) {
                    if (!autoResolve) {
                        _eWrongSolutionPuzzle?.Invoke();
                    }
                    return;
                }
            }

            _eResolvePuzzle?.Invoke();
            onResolve?.Invoke();
        }
    }
}