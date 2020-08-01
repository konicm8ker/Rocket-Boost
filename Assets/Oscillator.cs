﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

	[SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
	[SerializeField] float period = 2f; // Dictates movement speed
	float movementFactor; // 0 for moved, 1 for fully moved
	Vector3 startingPos;

	// Use this for initialization
	void Start () {
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// Prevents NaN error when period is 0
		if(period <= Mathf.Epsilon) { return; }
		float cycles = Time.time / period; // Grows continually from 0
		const float tau = Mathf.PI * 2f; // About 6.28
		float rawSinWave = Mathf.Sin(cycles * tau); // Goes from -1 to +1

		movementFactor = (rawSinWave / 2f) + 0.5f;
		Vector3 offset = movementVector * movementFactor;
		transform.position = startingPos + offset;

	}
}
