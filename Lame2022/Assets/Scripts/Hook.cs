using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {


	Fish fish = null;

	Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (fish || !other.gameObject.CompareTag("fish")) return;

		fish= other.gameObject.GetComponent<Fish>();
		fish.fishCollider.enabled = false;
		fish.rb.isKinematic = true;
		fish.transform.parent = transform;
		fish.transform.localPosition = Vector3.zero;
		
		fish.rb.velocity = Vector2.zero;
		rb.velocity = Vector2.zero;

	}

	public void CatchFish() {
		if (!fish) return;
		Destroy(fish.gameObject);
		Score.AddScore(1);
	}
}
