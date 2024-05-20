using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Serialization;

public class ClosePointTestScript : MonoBehaviour {


    public GameObject sourceObject;
    public GameObject point;
    public Collider targetCollider;
    public float offset = 5f;

    public GameObject fallwalls;

    private KinematicCharacterMotor motor;

    private void Start() {
        motor = GetComponent<KinematicCharacterMotor>();
    }


    private void Update() {
        fallwalls.transform.position = transform.position;
        
        Vector3 closestPoint = targetCollider.ClosestPoint(sourceObject.transform.position);

        Debug.Log(motor.GroundingStatus.GroundCollider.name);
        point.transform.position = closestPoint;
    }
}
