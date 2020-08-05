using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

	[SerializeField] float rcsThrust = 250f;
	[SerializeField] float mainThrust = 35f;
	[SerializeField] float levelLoadDelay = 3f;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip success;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem deathParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem damageParticles;

	Rigidbody rb;
	AudioSource rocketAudio;
	int currentLevel;

	enum State {Alive, Dying, Transcending};
	State state = State.Alive;

	// Use this for initialization
	void Start()
	{
		currentLevel = 1;
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
			case "Level3":
				LoadLevel3();
				break;
			case "Level4":
				LoadLevel4();
				break;
			case "Level5":
				LoadLevel5();
				break;
			case "Level6":
				LoadLevel6();
				break;
            default:
                StartDeathSequence();
                break;
        }
	}

	///////////////////////////////////////////////////////////////////////

    private void LoadLevel3()
    {
        print("Finished level.");
		rocketAudio.Stop(); // Stop all sounds before success sequence
        rocketAudio.PlayOneShot(success, 0.5f);
		successParticles.Play();
		transform.localScale = new Vector3(0,0,0); // Hide rocket ship
		mainEngineParticles.Stop(); // Hide mainEngine particles
        state = State.Transcending;
        Invoke("LoadLevelThree", levelLoadDelay);
    }

	private void LoadLevelThree()
	{
		currentLevel = 3;
		SceneManager.LoadScene("Level 3");
	}

	private void LoadLevel4()
    {
        print("Finished level.");
		rocketAudio.Stop(); // Stop all sounds before success sequence
        rocketAudio.PlayOneShot(success, 0.5f);
		successParticles.Play();
		transform.localScale = new Vector3(0,0,0); // Hide rocket ship
		mainEngineParticles.Stop(); // Hide mainEngine particles
        state = State.Transcending;
        Invoke("LoadLevelFour", levelLoadDelay);
    }

	private void LoadLevelFour()
	{
		currentLevel = 4;
		SceneManager.LoadScene("Level 4");
	}

	private void LoadLevel5()
    {
        print("Finished level.");
		rocketAudio.Stop(); // Stop all sounds before success sequence
        rocketAudio.PlayOneShot(success, 0.5f);
		successParticles.Play();
		transform.localScale = new Vector3(0,0,0); // Hide rocket ship
		mainEngineParticles.Stop(); // Hide mainEngine particles
        state = State.Transcending;
        Invoke("LoadLevelFive", levelLoadDelay);
    }

	private void LoadLevelFive()
	{
		currentLevel = 5;
		SceneManager.LoadScene("Level 5");
	}

	private void LoadLevel6()
    {
        print("Finished level.");
		rocketAudio.Stop(); // Stop all sounds before success sequence
        rocketAudio.PlayOneShot(success, 0.5f);
		successParticles.Play();
		transform.localScale = new Vector3(0,0,0); // Hide rocket ship
		mainEngineParticles.Stop(); // Hide mainEngine particles
        state = State.Transcending;
        Invoke("LoadLevelSix", levelLoadDelay);
    }

	private void LoadLevelSix()
	{
		currentLevel = 6;
		SceneManager.LoadScene("Level 6");
	}

	//////////////////////////////////////////////////////////////////////////

    private void StartDeathSequence()
    {
        print("You DIED."); // DEBUG
        rocketAudio.Stop();
		mainEngineParticles.Stop();
        rocketAudio.PlayOneShot(death, 0.5f);
		deathParticles.Play();
		damageParticles.Play();
        state = State.Dying;
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        print("Finished level.");
		rocketAudio.Stop(); // Stop all sounds before success sequence
        rocketAudio.PlayOneShot(success, 0.5f);
		successParticles.Play();
		transform.localScale = new Vector3(0,0,0); // Hide rocket ship
		mainEngineParticles.Stop(); // Hide mainEngine particles
        state = State.Transcending;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
		currentLevel = 2;
        SceneManager.LoadScene("Level 2"); // Allow for more than 2 levels
    }

	private void LoadFirstLevel()
    {
		currentLevel = 1;
        SceneManager.LoadScene("Level 1");
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