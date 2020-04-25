using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    // Private.
    Rigidbody rigidBody;
    AudioSource audioSource;
	GameState state = GameState.Alive;

    // Public.
	[SerializeField] float mainTrust = 10.0f; // Main trust variable.
	[SerializeField] float rcsTrust = 10.0f; // Left right rotation trust variable.

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
        Trust();
        Rotate();        
	}

    // Rocket rotation.
    private void Rotate()
    {
        // Take manual control of rotation. Freeze rotation.
        rigidBody.freezeRotation = true;

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

        // Take manual control of rotation. Un-Freeze rotation. Resume physics controll of rotation.
        rigidBody.freezeRotation = false;
    }

    // Rocket trust.
    private void Trust() {
        // Check if boost button was pressed.
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
			rigidBody.AddRelativeForce(Vector3.up * mainTrust);

            // Start audio when trust is engaged.
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
        else {
            // Stop audio when trust is stoped.
            audioSource.Stop();
        }
    }

	// Player collided with world object.
	void OnCollisionEnter(Collision collision) {
		// Prevent player controlling rocket when he died or is loading new level.
		if (state != GameState.Alive) {
			return;
		}
		switch (collision.gameObject.tag) {
		case Constants.FRIENDLY_TAG:
			print ("Friendly collision");
			break;
		case Constants.FINISH_TAG:
			state = GameState.Transcending;
			audioSource.Stop();
			Invoke ("LoadNextScene", 1.5f);
			break;
		default:
			state = GameState.Dying;
			audioSource.Stop();
			Invoke ("LoadNextScene", 1.5f);
			break;
		}
	}

	// Load next scene.
	private void LoadNextScene() {
		switch (state) {
		case GameState.Transcending:
			SceneManager.LoadScene (1);
			break;
		case GameState.Dying:
			SceneManager.LoadScene (0);
			break;
		}
	}		
}
