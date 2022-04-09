using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerScript : MonoBehaviour {
	public float speed = 1;
	public Transform mousebox;
	public SpriteShapeController line;
	public float lineSegmentLength = 10;

	public Rigidbody2D hook;
	public Transform Rod;
	public Transform RodEnd;


	[Range(0, 90)]
	public float RodMaxAngle = 20;
	public float LinePointDistance = .5f;
	public int LinePointCount = 20;
	public float LineFloatRate = 1;
	public float LineWhipRate = 1;


	List<Vector2> hookPoints = new List<Vector2>();
	// Vector3[] hookPoints = new Vector3[]{
	// 	Vector3.up * 1,
	// 	Vector3.up * 2,
	// 	Vector3.up * 3,
	// 	Vector3.up * 4,
	// 	Vector3.up * 5,
	// };

	void Start() {
		for (int i = 0; i < LinePointCount; i++) {
			hookPoints.Add(Vector3.up * 1 * .2f);
		}
	}

	private void FixedUpdate() {
		hook.AddForce(Vector2.up);
	}

	void Update() {



		float x = 0;
		float y = 0;

		Vector2 mouseposition = Pointer.current.position.ReadValue();
		var mouseboxposition = new Vector3(mouseposition.x, mouseposition.y, 0);
		mouseboxposition = Camera.main.ScreenToWorldPoint(mouseboxposition);
		mouseboxposition.z = 0;
		mousebox.position = mouseboxposition;

		float rodNewAngle = Vector3.SignedAngle(Vector3.up, mouseboxposition - Rod.position, Vector3.back);
		rodNewAngle = Mathf.Sign(rodNewAngle) * Mathf.Min(Mathf.Abs(rodNewAngle), RodMaxAngle);

		Rod.rotation = Quaternion.AngleAxis(rodNewAngle, Vector3.back);

		hookPoints[0] = RodEnd.position;

		for (int i = hookPoints.Count - 2; i >= 0; i--) {
			Vector3 prevPos = hookPoints[i + 1];
			Vector3 oldPos = hookPoints[i];
			if (Vector3.Distance(oldPos, prevPos) > LinePointDistance) {
				hookPoints[i] = prevPos + (oldPos - prevPos).normalized * LinePointDistance;
			}
		}

		for (int i = 1; i < hookPoints.Count; i++) {

			// hookPoints[i] += Vector3.up * LineFloatRate * Time.deltaTime;

			Vector3 prevPos = hookPoints[i - 1];
			Vector3 oldPos = hookPoints[i];
			if (Vector3.Distance(oldPos, prevPos) > LinePointDistance) {
				hookPoints[i] = prevPos + (oldPos - prevPos).normalized * LinePointDistance;
			}
		}

		if (Vector3.Distance(hook.position, hookPoints[hookPoints.Count - 1]) > LinePointDistance) {
			// hookPoints[i] = prevPos + (oldPos - prevPos).normalized * LinePointDistance;
			Vector2 delta = (hook.position - hookPoints[hookPoints.Count - 1]);
			hook.MovePosition(hookPoints[hookPoints.Count - 1] + delta.normalized * LinePointDistance);
			hook.velocity = delta * LineWhipRate;
		}
		// TODO: only update hook when "tugged"
		// TODO: tug line in reverse too, from hook (but prioritize rod)
		// TODO: add velocity to hook when tugged by line

		if (Keyboard.current.wKey.isPressed) {
			y += speed * Time.deltaTime;
			hook.AddForce(Vector2.up * 10);
		}
		if (Keyboard.current.aKey.isPressed) {
			x -= speed * Time.deltaTime;
		}
		if (Keyboard.current.sKey.isPressed) {
			y -= speed * Time.deltaTime;
		}
		if (Keyboard.current.dKey.isPressed) {
			x += speed * Time.deltaTime;
		}

		var position = transform.position;
		position.x += x;
		position.y += y;
		transform.position = position;

		line.spline.Clear();
		Vector3 lastPos = RodEnd.position;
		line.spline.InsertPointAt(0, lastPos);

		foreach (var item in hookPoints) {
			if (Vector3.Distance(lastPos, item) > .1f)
				line.spline.InsertPointAt(0, item);

			lastPos = item;
		}
		// if (Vector3.Distance(lastPos, hook.position) > .1f)
		// line.spline.InsertPointAt(0, hook.position);
		//float randomheight =Random.RandomRange(0.8f, 1.2f);
		// var scale = transform.localScale;
		// scale.y = randomheight;
		// transform.localScale = scale;

	}
}
