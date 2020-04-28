using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    // Private.
    private Rigidbody rigidBody;
    private AudioSource audioSource;
	private GameState state = GameState.Alive;

    // Public.
	[SerializeField] float mainTrust = 10.0f; // Main trust variable.
	[SerializeField] float rcsTrust = 10.0f; // Left right rotation trust variable.
	[SerializeField] float levelLoadDelay = 2.0f;
	[SerializeField] AudioClip mainEngine; // Main engine sound.
	[SerializeField] AudioClip success; // Player finished level sound.
	[SerializeField] AudioClip death; // Player lost sound.
	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem deathParticles;

	// Use this for initialization
	void Start () {
        // Find ships rigidbody reference.
        rigidBody = GetComponent<Rigidbody>();
       
        // Find audiosource.
        audioSource = GetComponent<AudioSource>();

		// Reset game state.
		state = GameState.Alive;
    }
	
	// Update is called once per frame
	void Update () {
		// Prevent player controlling rocket when he died or is loading new level.
		if (state != GameState.Alive) {
			return;
		}
        RespondToThrustInput();
        RespondToRotateInput();        
	}

    // Rocket rotation.
    private void RespondToRotateInput()
    {
        // Remove rotation due to physics.
		rigidBody.angularVelocity = Vector3.zero;

		// Trust rcs.
		float rotationThisFrame = rcsTrust * Time.deltaTime;

        // Check if left or right button was pressed.
        if (Input.GetKey(KeyCode.A))
        {
			transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if(Input.GetKey(KeyCode.D)) 
        {
			transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }

    // Rocket trust.
    private void RespondToThrustInput() {
        // Check if boost button was pressed.
		bool playRocketEngineAudioSource = false;
		if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.W)) {
			ApplyThrust ();
			playRocketEngineAudioSource = true;
		}
		RocketEngine (playRocketEngineAudioSource);
		MainEngineParticles (playRocketEngineAudioSource);
    }

	// Main engine particle.
	private void MainEngineParticles(bool play) {
		switch (play) {
		case true:
			mainEngineParticles.Play (); // Play engine particles.
			break;
		case false:
			mainEngineParticles.Stop (); // Stop engine particles.
			break;
		}
	}

	// Apply thrust to rocket.
	private void ApplyThrust() {
		rigidBody.AddRelativeForce(Vector3.up * mainTrust * Time.deltaTime);
	}
		
	// Player collided with world object.
	void OnCollisionEnter(Collision collision) {
		// Prevent player controlling rocket when he died or is loading new level.
		if (state != GameState.Alive) {
			return;
		}
		switch (collision.gameObject.tag) {
		case Constants.FRIENDLY_TAG:
			// Do nothing.
			break;
		case Constants.FINISH_TAG:
			StartSuccessSequence ();
			break;
		default:
			StartDeathSequence ();
			break;
		}
	}

	// Start success sequence.
	private void StartSuccessSequence() {
		state = GameState.Transcending;
		RocketEngine (false);
		PlayAudio (success);
		successParticles.Play ();
		MainEngineParticles (false); // Stop engine particles.
		Invoke ("LoadNextScene", levelLoadDelay);
	}

	// Start death sequence.
	private void StartDeathSequence() {
		state = GameState.Dying;
		RocketEngine (false);
		PlayAudio (death);
		deathParticles.Play ();
		MainEngineParticles (false); // Stop engine particles.
		Invoke ("LoadNextScene", levelLoadDelay);
	}

	// Handle audio.
	private void RocketEngine(bool isPlaying) {
		switch (isPlaying) {
		case true:
			// Start audio when trust is engaged.
			if (!audioSource.isPlaying) {
				PlayAudio (mainEngine);
			}
			break;
		case false:
			// Stop audio when trust is stoped.
			audioSource.Stop ();
			break;
		}
	}

	// Play Audio one shot.
	private void PlayAudio(AudioClip clip) {
		audioSource.Stop (); // Stop previously played sound.
		audioSource.PlayOneShot (clip); // Play new audio clip.
	}

	// Load next scene.
	private void LoadNextScene() {
		switch (state) {
		case GameState.Transcending:
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			int nextSceneIndex = currentSceneIndex + 1;
			if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
				nextSceneIndex = 0; // Loop back to first scene.
			}
			SceneManager.LoadScene (nextSceneIndex);
			break;
		case GameState.Dying:
			SceneManager.LoadScene (0);
			break;
		}
	}		
}
