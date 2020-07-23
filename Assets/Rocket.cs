using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rb;
	AudioSource thrustSound;
	bool m_Play;
	bool m_ToggleChange;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		thrustSound = GetComponent<AudioSource>();

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

			if(!thrustSound.isPlaying) {
				thrustSound.Play();
			}
		} else {
			thrustSound.Stop();
		}

		if(Input.GetKey(KeyCode.A)) {
			print("Rotating left.");  // DEBUG
			transform.Rotate(Vector3.forward);
		} else if(Input.GetKey(KeyCode.D)) {
			print("Rotating right.");  // DEBUG
			transform.Rotate(-Vector3.forward);
		}
    }
}
