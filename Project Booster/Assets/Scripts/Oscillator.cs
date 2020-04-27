using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // There can be only one script of this kind on gameobject.
public class Oscillator : MonoBehaviour {

	// Public.
	[SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
	[SerializeField] float period = 2f;

	// Private.
	float movementFactor; // 0 for not moved, 1 for fully moved.
	private Vector3 startingPos;

	// Use this for initialization
	void Start () {
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// Protect from dividing by zero. Mathf.Epsilon is almost zero.
		if (period <= Mathf.Epsilon) {return;}

		// Set movement factor.
		float cycles = Time.time / period; // Grows continualy from 0.
		const float tau = Mathf.PI * 2f; // About 6.28f.
		float rawSinWave = Mathf.Sin(cycles * tau); // Goes from -1 to +1.
		movementFactor = (rawSinWave / 2f) + 0.5f; // Goes from 0 to 1.
		Vector3 offset = movementFactor * movementVector;
		transform.position = startingPos + offset;
	}
}
