using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {

	public Rigidbody2D rb;
	public Vector2 speed;

	// Start is called before the first frame update
	void Start() {
		rb.velocity = speed;

	}

	// Update is called once per frame
	void Update() {

	}
}
