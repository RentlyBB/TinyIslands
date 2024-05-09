using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private Rigidbody _rigidbody;

    public Vector3 dir = Vector3.zero;

    public float movementSpeed = 100f;

    // Start is called before the first frame update
    void Start() {
        transform.TryGetComponent<Rigidbody>(out _rigidbody);

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.W)) {
            dir.x = -1;
        } else if (Input.GetKey(KeyCode.A)) {
            dir.z = -1;
        } else if (Input.GetKey(KeyCode.S)) {
            dir.x = 1;
        } else if (Input.GetKey(KeyCode.D)) {
            dir.z = 1;
        } else {
            dir = Vector3.zero;
        }

    }

    private void FixedUpdate() {

        if (dir.x != 0 || dir.z != 0) { 
            _rigidbody.AddForce(dir * movementSpeed);
        }
    }
}