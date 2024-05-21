using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Serialization;

public class ClosePointTestScript : MonoBehaviour {
    public Transform fallWallPoints;
    public Transform fallWalls;

    public LayerMask layerMask = 1 << 8;

    private List<Transform> _wallPoints = new List<Transform>();
    private List<Transform> _fallWalls = new List<Transform>();
    private RaycastHit _hit;


    private KinematicCharacterMotor _motor;

    private void Start() {
        _motor = GetComponent<KinematicCharacterMotor>();
        layerMask = ~layerMask;

        fallWallPoints.position = transform.position;
        fallWalls.position = transform.position;

        foreach (Transform wallPoint in fallWallPoints) {
            _wallPoints.Add(wallPoint);
        }

        foreach (Transform fallWall in fallWalls) {
            _fallWalls.Add(fallWall);
        }

        Debug.Log(_wallPoints);
    }

    private void Update() {
        // WallPositioning();
        SetWallPosition();
    }

    // private void WallPositioning() {
    //     fallWallPoints.position = transform.position;
    //
    //     if (!_motor.GroundingStatus.GroundCollider) return;
    //
    //     //Last working version
    //     for (int i = 0; i < _wallPoints.Count; i++) {
    //         Vector3 closestPoint = _motor.GroundingStatus.GroundCollider.ClosestPoint(_wallPoints[i].position);
    //         Vector3 targetWallPos = new Vector3(closestPoint.x, transform.position.y + 1, closestPoint.z);
    //
    //         _fallWalls[i].position = targetWallPos;
    //         _fallWalls[i].rotation = _wallPoints[i].rotation;
    //     }
    // }

    //TODO: FIXED -> need to be tested
    private void SetWallPosition() {
        fallWallPoints.position = transform.position;

        if (!_motor.GroundingStatus.GroundCollider) return;

        for (int i = 0; i < _wallPoints.Count; i++) {
            Vector3 closestPoint = _motor.GroundingStatus.GroundCollider.ClosestPointOnBounds(_wallPoints[i].position);
            Vector3 targetWallPos = new Vector3(closestPoint.x, transform.position.y + 1, closestPoint.z);

            if (!Physics.Raycast(_wallPoints[i].position, _wallPoints[i].TransformDirection(Vector3.down), out _hit, 1.5f, layerMask)) {
                Debug.DrawRay(_wallPoints[i].position, _wallPoints[i].TransformDirection(Vector3.down) * 1.5f, Color.yellow);
                _fallWalls[i].position = targetWallPos;
            } else {
                fallWalls.position = transform.position;
                Debug.DrawRay(_wallPoints[i].position, _wallPoints[i].TransformDirection(Vector3.down) * 1.5f, Color.red);
            }

            //_fallWalls[i].rotation = _wallPoints[i].rotation;
        }
    }

    private void FixedUpdate() {
        // foreach (Transform wallPoint in _wallPoints) {
        //     if (Physics.Raycast(wallPoint.position, wallPoint.TransformDirection(Vector3.down), out _hit, 1.5f, layerMask)) {
        //         Debug.DrawRay(wallPoint.position, wallPoint.TransformDirection(Vector3.down) * 1.5f, Color.yellow);
        //     } else {
        //         Debug.DrawRay(wallPoint.position, wallPoint.TransformDirection(Vector3.down) * 1.5f, Color.red);
        //     }
        // }
    }
}