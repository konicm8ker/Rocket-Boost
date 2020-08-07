using UnityEngine;

public class CameraController2 : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float offset = 14f;
	[SerializeField] float speed = 0.25f;

	void LateUpdate()
	{
		if(target.position.y > (transform.position.y + offset) && !(transform.position.y > 289)) // If rocket hits top trigger
		{
			transform.position = new Vector3(transform.position.x,(transform.position.y + speed),transform.position.z);
		}
		else if((target.position.y < (transform.position.y - offset)) && !(transform.position.y < 22)) // If rocket hits bottom trigger
		{
			transform.position = new Vector3(transform.position.x,(transform.position.y - speed),transform.position.z);
		}

	}

}