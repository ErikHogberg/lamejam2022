using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {

	public Rigidbody2D rb;
	public Vector2 speed;
	float spawntimer = 1;
	// Start is called before the first frame update
	void Start() {
		rb.velocity = speed;

	}

	// Update is called once per frame
	void Update() {
		if (rb.velocity.magnitude < 1f) {
			rb.velocity = speed;

		}
		spawntimer -= Time.deltaTime;

	}
	private void OnCollisionEnter2D(Collision2D collision) {
		if (spawntimer < 0) {
		Instantiate(this);
			spawntimer = 1;

		}
		
	}
}
