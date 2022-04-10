﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {


	const int Limit = 30;

	public Rigidbody2D rb;
	public Collider2D fishCollider;
	public Vector2 speed;
	public float velocityCap = 10f;
	float spawntimer = 1;
	// Start is called before the first frame update

	public Sprite[] sprites;
	SpriteRenderer spriteRenderer;

	public static List<Fish> AllFish = new List<Fish>();

	void Start() {

		AllFish.Add(this);

		rb.velocity = speed;
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length - 1)];
	}

	private void OnDestroy() {
		AllFish.Remove(this);
		if (AllFish.Count < 1) {
			if (menuStart.MainInstance)
				menuStart.MainInstance.GameOverScreen();
		}
	}

	private void FixedUpdate() {
		if (rb.velocity.sqrMagnitude > velocityCap * velocityCap) {
			rb.velocity = rb.velocity.normalized * velocityCap;
		}
	}

	void Update() {
		if (rb.velocity.magnitude < 1f) {
			rb.velocity = speed;

		}
		spawntimer -= Time.deltaTime;

	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.gameObject.CompareTag("fish")) return;
		collision.gameObject.GetComponent<Fish>().RestartTimer();
		if (spawntimer < 0 && AllFish.Count < Limit) {
			var fish = Instantiate(this, transform.parent);
			spawntimer = 1;
		FindObjectOfType<AudioManager>().Play("FishCollision");
		}

	}
	public void RestartTimer() {
		spawntimer = 1;
	}
}
