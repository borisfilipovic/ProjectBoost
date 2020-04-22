using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    // Private.
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Public.

	// Use this for initialization
	void Start () {
        // Find ships rigidbody reference.
        rigidBody = GetComponent<Rigidbody>();

        // Find audiosource.
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    // Handle user imputs.
    private void ProcessInput()
    {
        // Check if boost button was pressed.
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);

            // Start audio when trust is engaged.
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
        else {
            // Stop audio when trust is stoped.
            audioSource.Stop();
        }

        // Check if left or right button was pressed.
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
            transform.Rotate(Vector3.forward);
        }
        else if(Input.GetKey(KeyCode.D)) 
        {
            print("Rotating right");
            transform.Rotate(-Vector3.forward);
        }
    }
}
