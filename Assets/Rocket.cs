using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

	[SerializeField] float rcsThrust = 250f;
	[SerializeField] float mainThrust = 35f;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip success;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem deathParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem damageParticles;

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
				print("OK."); // DEBUG
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
        print("You DIED."); // DEBUG
        rocketAudio.Stop();
		mainEngineParticles.Stop();
        rocketAudio.PlayOneShot(death);
		deathParticles.Play();
		damageParticles.Play();
        state = State.Dying;
        Invoke("LoadFirstLevel", 3f); // Parameterize time
    }

    private void StartSuccessSequence()
    {
        print("Finished level.");
		rocketAudio.Stop(); // Stop all sounds before success sequence
        rocketAudio.PlayOneShot(success);
		successParticles.Play();
		transform.localScale = new Vector3(0,0,0); // Hide rocket ship
		mainEngineParticles.Stop(); // Hide mainEngine particles
        state = State.Transcending;
        Invoke("LoadNextLevel", 3f); // Parameterize time
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
			mainEngineParticles.Stop();
		}
    }

    private void EngageThrust()
    {
        print("Thrust engaged!"); // DEBUG
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!rocketAudio.isPlaying)
        {
            rocketAudio.PlayOneShot(mainEngine);
        }
		mainEngineParticles.Play();
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