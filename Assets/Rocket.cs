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
	enum State {Alive, Dying, Transcending};
	State state = State.Alive;
	bool collisionsDetected = true;

	// Use this for initialization
	void Start()
	{
		print("Lives: " + PlayerStats.Lives);
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
		// Only run in debug mode
		if(Debug.isDebugBuild)
		{
			RespondToDebugInput();
		}

	}

    public void RespondToDebugInput()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			int nextScene = currentSceneIndex + 1;
			int sceneCount = SceneManager.sceneCountInBuildSettings;

            if (nextScene == sceneCount) { nextScene = 0; }
            SceneManager.LoadScene(nextScene);
        }
        else if(Input.GetKeyDown(KeyCode.C))
		{
			collisionsDetected = !collisionsDetected;
		}
    }

    void OnCollisionEnter(Collision collision)
	{
		if(state != State.Alive || collisionsDetected == false) { return; } // Ignore collisons when dead

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
		if(PlayerStats.Lives > 0)
		{
			print("You DIED."); // DEBUG
			rocketAudio.Stop();
			mainEngineParticles.Stop();
			rocketAudio.PlayOneShot(death, 0.5f);
			deathParticles.Play();
			damageParticles.Play();
			state = State.Dying;
			PlayerStats.Lives -= 1;
			Invoke("RestartCurrentLevel", levelLoadDelay);
		}
		else
		{
			print("You DIED."); // DEBUG
			rocketAudio.Stop();
			mainEngineParticles.Stop();
			rocketAudio.PlayOneShot(death, 0.5f);
			deathParticles.Play();
			damageParticles.Play();
			state = State.Dying;
			PlayerStats.Lives = 3;
			Invoke("LoadFirstLevel", levelLoadDelay);
		}
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
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextScene = currentSceneIndex + 1;
		int sceneCount = SceneManager.sceneCountInBuildSettings;

		if(nextScene == sceneCount) { nextScene = 0; }	
		SceneManager.LoadScene(nextScene);
        
    }

	private void LoadFirstLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

	private void RestartCurrentLevel()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex);
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