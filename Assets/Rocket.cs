using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Fix lighting bug when loading scene

public class Rocket : MonoBehaviour
{

	[SerializeField] float rcsThrust = 250f;
	[SerializeField] float mainThrust = 40f;
	Rigidbody rb;
	AudioSource thrustSound;

	enum State {Alive, Dying, Transcending};
	State state = State.Alive;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		thrustSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
	{
		// Somewhere stop thrust sound when dying
		if(state == State.Alive)
		{
			Thrust();
			Rotate();
		}
	}

    void OnCollisionEnter(Collision collision)
	{
		if(state != State.Alive) { return; } // Ignore collisons when dead

		switch(collision.gameObject.tag)
		{
			case "Friendly":
				print("OK.");
				break;
            case "Finish":
                print("Finished level.");
				state = State.Transcending;
                Invoke("LoadNextLevel", 1f); // parameterize time
                break;
            default:
                print("You DIED.");
				state = State.Dying;
                Invoke("LoadFirstLevel", 3f); // parameterize time
                break;
        }
	}

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // Allow for more than 2 levels
    }

	private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
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