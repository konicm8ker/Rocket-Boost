using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour {

	[SerializeField] AudioSource wallSound;

	// Use this for initialization
	void Start () {
		wallSound = GetComponent<AudioSource>();
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Wall" && Time.time > 1)
		{
			if(!wallSound.isPlaying) { wallSound.Play(); }	
		}
	}
}
