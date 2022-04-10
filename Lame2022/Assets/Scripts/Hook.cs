using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {


	GameObject fish = null;

	private void OnCollisionEnter2D(Collision2D other) {
		if (fish || !other.gameObject.CompareTag("fish")) return;

		fish = other.gameObject;
		fish.GetComponent<Rigidbody2D>().isKinematic = true;
		fish.transform.parent = transform;
	}

	public void CatchFish() {
		if (!fish) return;
		Destroy(fish);
		Score.AddScore(1);
	}
}
