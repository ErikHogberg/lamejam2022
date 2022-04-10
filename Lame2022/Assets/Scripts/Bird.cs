using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	const int Limit = 10;

	public static List<Bird> AllBirds = new List<Bird>();
	public static int BirdCount => AllBirds.Count;
	public static bool CanSpawn => BirdCount < Limit;


	private Fish target;
	bool flyingOut = false;
	bool holdingFish = false;

	Rigidbody2D rb;

	Camera cam;

	public Vector2 FlyInSpeed;
	public Vector2 FlyOutSpeed;

	public static void Spawn(Bird birdToSpawn, Fish target, Vector2 where) {
		if (!CanSpawn) return;

		Bird bird = Instantiate(birdToSpawn);
		Vector3 birdPos = bird.transform.position;
		birdPos.x = where.x;
		birdPos.y = where.y;
		bird.transform.position = birdPos;
		bird.target = target;
		bird.cam = Camera.main;
	}

	void Start() {
		AllBirds.Add(this);
		cam = Camera.main;
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnDestroy() {
		AllBirds.Remove(this);
	}

	private void FixedUpdate() {
		if (!flyingOut) {

		} else {

		}
	}

	private void OnCollisionEnter(Collision other) {
		if (flyingOut && !holdingFish) return;

		if (other.gameObject.CompareTag("player")) {
			// rb.isKinematic = false;
			flyingOut = true;
			rb.angularVelocity = 100f;
		} else
		if (!holdingFish && other.gameObject.CompareTag("fish")) {
			flyingOut = true;
			holdingFish = true;
			Fish fish = other.gameObject.GetComponent<Fish>();
			fish.rb.isKinematic = true;
			fish.fishCollider.enabled = false;
			fish.transform.parent = transform;
		}

	}
}
