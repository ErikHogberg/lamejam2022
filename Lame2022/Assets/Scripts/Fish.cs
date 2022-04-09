using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {

	public Rigidbody2D rb;
	public Vector2 speed;
	float spawntimer = 1;
	// Start is called before the first frame update

	public Sprite[] sprites;
	SpriteRenderer spriteRenderer;

	void Start() {
		rb.velocity = speed;
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length-1)]; 
	}

	// Update is called once per frame
	void Update() {
		if (rb.velocity.magnitude < 1f) {
			rb.velocity = speed;

		}
		spawntimer -= Time.deltaTime;

	}
	private void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.gameObject.CompareTag("fish"))return;
		collision.gameObject.GetComponent<Fish>().RestartTimer();
		if (spawntimer < 0) {
		var fish= Instantiate(this,transform.parent);
			spawntimer = 1;

		}
		
	}
	public void RestartTimer() {
		spawntimer = 1;
	}
}
