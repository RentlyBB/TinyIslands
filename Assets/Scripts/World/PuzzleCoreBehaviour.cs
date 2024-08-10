using System.Collections.Generic;
using System.Linq;
using EditorScripts;
using UnityEngine;
using UnityEngine.Events;
using World.Interfaces;

namespace World {
    public class PuzzleCoreBehaviour : MonoBehaviour {
        [TextArea]
        public string description;

        [Space]
        public bool autoResolve;

        [Space]
        public List<GameObject> puzzlesObj = new();

        public UnityEvent onResolve;

        private readonly UnityEvent _eCheckPuzzle = new();
        private readonly UnityEvent _eResolvePuzzle = new();
        private readonly UnityEvent _eWrongSolutionPuzzle = new();
        private readonly List<IPuzzle> _iPuzzles = new();

        private void Start() {
            if (puzzlesObj.Count != 0)
                foreach (var puz in puzzlesObj.Select(puzzle => puzzle.GetComponent<IPuzzle>())) {
                    _iPuzzles.Add(puz);
                    _eCheckPuzzle.AddListener(puz.CheckPuzzle);
                    _eResolvePuzzle.AddListener(puz.ResolvePuzzle);
                    _eWrongSolutionPuzzle.AddListener(puz.WrongSolution);
                }
        }

        private void Update() {
            if (autoResolve) ResolveAllPuzzles();
        }

        [InvokeButton]
        public void ResolveAllPuzzles() {
            if (puzzlesObj.Count == 0) return;

            _eCheckPuzzle?.Invoke();

            // If one puzzle is wrong, return it
            foreach (var puz in _iPuzzles)
                if (!puz.IsSolved()) {
                    if (!autoResolve) _eWrongSolutionPuzzle?.Invoke();
                    return;
                }

            _eResolvePuzzle?.Invoke();
            onResolve?.Invoke();
        }
    }
}