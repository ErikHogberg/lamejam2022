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
	public float HookVelocityCap = 10f;
	public Transform Rod;
	public Transform RodEnd;

	[Space]
	public Camera ZoomCamera;
	public AnimationCurve ZoomCurve = AnimationCurve.Linear(0, 0, 1, 1);

	public Vector2 ZoomRange = new Vector2(5, 20);
	public float ZoomSpeed = .1f;
	float zoomProgress = .5f;

	[Range(0, 90)]
	public float RodMaxAngle = 20;
	public float LinePointDistance = .5f;
	public int LinePointCount = 20;
	public float LineFloatRate = 1;
	public float LineWhipRate = 1;


	List<Vector2> hookPoints = new List<Vector2>();

	void Start() {
		// subdivide fishing line
		for (int i = 0; i < LinePointCount; i++) {
			hookPoints.Add(Vector3.up * 1 * .2f);
		}

		if (!ZoomCamera) ZoomCamera = Camera.main;
	}

	private void FixedUpdate() {
		// make hook float upwards
		// hook.AddForce(Vector2.up * LineFloatRate, ForceMode2D.Force);
		if (hook.velocity.sqrMagnitude > HookVelocityCap * HookVelocityCap) {
			hook.velocity = hook.velocity.normalized * HookVelocityCap;
		}
	}

	void Update() {

		float scroll = -Mouse.current.scroll.y.ReadValue();
		zoomProgress += scroll * ZoomSpeed;
		zoomProgress = Mathf.Clamp01(zoomProgress);

		float zoomEval = Mathf.Lerp(ZoomRange.x, ZoomRange.y, ZoomCurve.Evaluate(zoomProgress));
		ZoomCamera.orthographicSize = zoomEval;

		// moves mouse box/circle to mouse
		Vector2 mouseposition = Pointer.current.position.ReadValue();
		var mouseboxposition = new Vector3(mouseposition.x, mouseposition.y, 0);
		mouseboxposition = Camera.main.ScreenToWorldPoint(mouseboxposition);
		mouseboxposition.z = 0;
		mousebox.position = mouseboxposition;

		// rotate fishing rod towards mouse
		float rodNewAngle = Vector3.SignedAngle(Vector3.up, mouseboxposition - Rod.position, Vector3.back);
		rodNewAngle = Mathf.Sign(rodNewAngle) * Mathf.Min(Mathf.Abs(rodNewAngle), RodMaxAngle);
		Rod.rotation = Quaternion.AngleAxis(rodNewAngle, Vector3.back);

		// tug fishing line towards hook
		if (Vector3.Distance(hook.position, hookPoints[hookPoints.Count - 1]) > LinePointDistance * .8f) {
			hookPoints[hookPoints.Count - 1] = hook.position + (hookPoints[hookPoints.Count - 1] - hook.position).normalized * LinePointDistance * .8f;
		}

		// tug other parts of line in order towards fishing rod
		for (int i = hookPoints.Count - 2; i >= 0; i--) {
			Vector3 prevPos = hookPoints[i + 1];
			Vector3 oldPos = hookPoints[i];
			if (Vector3.Distance(oldPos, prevPos) > LinePointDistance) {
				hookPoints[i] = prevPos + (oldPos - prevPos).normalized * LinePointDistance;
			}
		}

		// tug fishing line towards fishing rod
		hookPoints[0] = RodEnd.position;
		for (int i = 1; i < hookPoints.Count; i++) {

			// hookPoints[i] += Vector3.up * LineFloatRate * Time.deltaTime;

			Vector3 prevPos = hookPoints[i - 1];
			Vector3 oldPos = hookPoints[i];
			if (Vector3.Distance(oldPos, prevPos) > LinePointDistance) {
				hookPoints[i] = prevPos + (oldPos - prevPos).normalized * LinePointDistance;
			}
		}

		// tug hook
		if (Vector3.Distance(hook.position, hookPoints[hookPoints.Count - 1]) > LinePointDistance) {
			Vector2 delta = (hook.position - hookPoints[hookPoints.Count - 1]);
			hook.MovePosition(hookPoints[hookPoints.Count - 1] + delta.normalized * LinePointDistance);
			// flick hook if it was tugged
			hook.AddForce(delta * LineWhipRate, ForceMode2D.Impulse);
		}

		// move player
		/*
		float x = 0;
		float y = 0;

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
		*/

		// update fishing line graphics
		line.spline.Clear();
		Vector3 lastPos = RodEnd.position;
		line.spline.InsertPointAt(0, lastPos);
		foreach (var item in hookPoints) {
			if (Vector3.Distance(lastPos, item) > .1f)
				line.spline.InsertPointAt(0, item);
			lastPos = item;
		}

		// wobble player
		//float randomheight =Random.RandomRange(0.8f, 1.2f);
		// var scale = transform.localScale;
		// scale.y = randomheight;
		// transform.localScale = scale;

	}
}
