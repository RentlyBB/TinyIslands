using System;
using UnityEngine;
using World.AbstractClasses;
using World.Interfaces;

namespace World {
    public class Collectible : Interactable {
    
        // Rotation speed in degrees per second
        public float rotationSpeed = 90f;

        // Amplitude of the sinusoidal movement
        public float amplitude = 0.5f;

        // Frequency of the sinusoidal movement
        public float frequency = 1f;

        // Original position of the coin
        private Vector3 _startPosition;
  
        private void Start()
        {
            // Save the starting position of the coin
            _startPosition = transform.position;
        }

        private void Update()
        {
            // Smooth rotation around the Y-axis
            RotateCoin();

            // Sinusoidal up and down movement
            SinusoidalMovement();
        }

        private void RotateCoin()
        {
            // Rotate the coin smoothly around the Y-axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        private void SinusoidalMovement()
        {
            // Calculate the new Y position using a sine wave
            float newY = _startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

            // Apply the new Y position to the coin
            transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
        }
        

        protected override void Interact() {
            Destroy(this.gameObject);
        }
    }
}