using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClosePointTestScript : MonoBehaviour {


    public GameObject sourceObject;
    public GameObject point;
    public Collider targetCollider;
    public float offset = 5f; 
    

   

    private void Update() {
        Vector3 closestPoint = targetCollider.ClosestPoint(sourceObject.transform.position);
        
        point.transform.position = closestPoint;
    }
}
