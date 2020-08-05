using UnityEngine;

public class WallCollision : MonoBehaviour {

	[SerializeField] AudioClip heavyBoom;
	AudioSource wallSound;

	// Use this for initialization
	void Start () {
		wallSound = GetComponent<AudioSource>();
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Wall" && Time.time > 1f)
		{
			wallSound.PlayOneShot(heavyBoom);
		}
	}
}
