using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    // Private.
    Rigidbody rigidBody;

    // Public.

	// Use this for initialization
	void Start () {
        // Find ships rigidbody reference.
        rigidBody = GetComponent<Rigidbody>();
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
        }

        // Check if left or right button was pressed.
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
        }
        else if(Input.GetKey(KeyCode.D)) 
        {
            print("Rotating right");
        }
    }
}
