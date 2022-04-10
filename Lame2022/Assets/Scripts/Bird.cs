using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	const int Limit = 10;

	public static List<Bird> AllBirds = new List<Bird>();
	public static int BirdCount => AllBirds.Count;
	public static bool CanSpawn => BirdCount < Limit;


	public Fish target;
	bool flyingOut = false;
	bool holdingFish = false;

	Rigidbody2D rb;

	Camera cam;

	[Min(0)]
	public float FlyInSpeed = 1;
	[Min(0)]
	public float FlyOutSpeed = 2;

	public float DespawnTime = 5;
	float despawnTimer = float.MaxValue;

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
		if (!flyingOut && target) {
			Vector2 targetDir = (target.transform.position - transform.position).normalized;
			rb.velocity = targetDir * FlyInSpeed;
			rb.angularVelocity = 0;
		} else {
			despawnTimer -= Time.fixedDeltaTime;
			if(despawnTimer < 0f) {
				Destroy(gameObject);
				Debug.Log("despawned bird");
				return;
			}
			
			Vector2 dir = (cam.WorldToViewportPoint(transform.position) - Vector3.one * .5f).normalized;
			rb.velocity = dir * FlyOutSpeed;
			
		}
	}

	private void OnCollisionEnter(Collision other) {
		if (flyingOut && !holdingFish) return;

		if (other.gameObject.CompareTag("Player")) {
			// rb.isKinematic = false;
			flyingOut = true;
			despawnTimer = DespawnTime;
			rb.angularVelocity = 100f;
			if (holdingFish) {
				target.rb.isKinematic = false;
				target.fishCollider.enabled = true;
				target.transform.parent = transform.parent;
			}
		} else
		if (!holdingFish && other.gameObject.CompareTag("fish")) {
			flyingOut = true;
			holdingFish = true;
			Fish fish = other.gameObject.GetComponent<Fish>();
			if (target != fish) target = fish;
			fish.rb.isKinematic = true;
			fish.fishCollider.enabled = false;
			fish.transform.parent = transform;
		}

	}
}
