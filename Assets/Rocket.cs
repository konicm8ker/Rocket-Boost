using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Fix lighting bug when loading scene

public class Rocket : MonoBehaviour
{

	[SerializeField] float rcsThrust = 250f;
	[SerializeField] float mainThrust = 40f;
	[SerializeField] AudioClip MainEngine;
	[SerializeField] AudioClip DeathSound;
	[SerializeField] AudioClip LevelComplete;
	Rigidbody rb;
	AudioSource rocketAudio;

	enum State {Alive, Dying, Transcending};
	State state = State.Alive;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rocketAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if(state == State.Alive)
		{
			RespondToThrustInput();
			RespondToRotateInput();
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
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
	}

    private void StartDeathSequence()
    {
        print("You DIED.");
        rocketAudio.Stop();
        rocketAudio.PlayOneShot(DeathSound);
        state = State.Dying;
        Invoke("LoadFirstLevel", 3f); // parameterize time
    }

    private void StartSuccessSequence()
    {
        print("Finished level.");
        rocketAudio.PlayOneShot(LevelComplete);
        state = State.Transcending;
        Invoke("LoadNextLevel", 3f); // parameterize time
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // Allow for more than 2 levels
    }

	private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
	{
        if(Input.GetKey(KeyCode.Space))
        {
            EngageThrust();
        }
        else
		{
			rocketAudio.Stop();
		}
    }

    private void EngageThrust()
    {
        print("Thrust engaged!"); // DEBUG
        rb.AddRelativeForce(Vector3.up * mainThrust);
        if (!rocketAudio.isPlaying)
        {
            rocketAudio.PlayOneShot(MainEngine);
        }
    }

    private void RespondToRotateInput()
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