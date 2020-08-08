using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float offset = 14f;
	[SerializeField] float speed = 0.25f;
	[SerializeField] float topBorder;
	[SerializeField] float bottomBorder;

	void LateUpdate()
	{
		if(target.position.y > (transform.position.y + offset) && !(transform.position.y > topBorder)) // If rocket hits top trigger DV: 72, 289
		{
			transform.position = new Vector3(transform.position.x,(transform.position.y + speed),transform.position.z);
		}
		else if((target.position.y < (transform.position.y - offset)) && !(transform.position.y < bottomBorder)) // If rocket hits bottom trigger DV: 22
		{
			transform.position = new Vector3(transform.position.x,(transform.position.y - speed),transform.position.z);
		}

	}

}
