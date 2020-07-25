using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Rocket : MonoBehaviour
{

	[SerializeField] float rcsThrust = 250f;
	[SerializeField] float mainThrust = 40f;
	Rigidbody rb;
	AudioSource thrustSound;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		thrustSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
	{
		Thrust();
		Rotate();
	}

    void OnCollisionEnter(Collision collision)
	{
		switch(collision.gameObject.tag)
		{
			case "Friendly":
				print("OK.");
				break;
			default:
				print("You DIED.");
				break;
		}
	}

    private void Thrust()
	{
        if(Input.GetKey(KeyCode.Space))
		{
			print("Thrust engaged!"); // DEBUG
			rb.AddRelativeForce(Vector3.up * mainThrust);
			if(!thrustSound.isPlaying)
			{
				thrustSound.Play();
			}
		}
		else
		{
			thrustSound.Stop();
		}
    }

	private void Rotate()
    {
		float rotationThisFrame = rcsThrust * Time.deltaTime;
		rb.freezeRotation = true;  // Take manual control of rotation

        if(Input.GetKey(KeyCode.A))
		{
			print("Rotating left.");  // DEBUG
			transform.Rotate(Vector3.forward * rotationThisFrame);
		}
		else if(Input.GetKey(KeyCode.D))
		{
			print("Rotating right.");  // DEBUG
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}

		rb.freezeRotation = false; // Resume physics control
    }

}