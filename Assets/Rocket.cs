using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput();
	}

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.Space)) {
			print("Thrust engaged!"); // DEBUG
			rb.AddRelativeForce(Vector3.up);
		}

		if(Input.GetKey(KeyCode.A)) {
			print("Rotating left.");  // DEBUG
		} else if(Input.GetKey(KeyCode.D)) {
			print("Rotating right.");  // DEBUG
		}
    }
}
