using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerScript : MonoBehaviour {
	public float speed = 1;
	public Transform mousebox;
	public SpriteShapeController line;
	public float lineSegmentLength = 10;

	public Hook hook;
	public Rigidbody2D hookRigidbody;
	public float HookVelocityCap = 10f;
	public Transform Rod;
	public Transform RodEnd;

	[Space]
	public Camera ZoomCamera;
	public AnimationCurve ZoomCurve = AnimationCurve.Linear(0, 0, 1, 1);

	public Vector2 ZoomRange = new Vector2(5, 20);
	public float ZoomSpeed = .1f;
	float zoomProgress = .5f;

	[Range(0, 180)]
	public float RodMaxAngle = 20;
	float rodOldAngle = 0;
	Vector3 rodEndOldPos = Vector3.zero;

	[Space]
	[Range(0, 1)]
	public float ReeledMul = .2f;
	public float ReelSpeed = .2f;

	[Min(0)]
	public float InstantReelThreshold = 1f;
	public AnimationCurve ReelCurve = AnimationCurve.Linear(0, 0, 1, 1);

	[Space]
	public float LinePointDistance = .5f;
	public float LineMinPointDistance = .5f;


	// [Min(0)]
	// public float MaxBendPerSec = 30f;

	[Min(3)]
	public int LinePointCount = 20;
	public float HookFloatRate = 1;
	public float LineFloatRate = 1;
	public float LineWhipRate = 1;


	List<Vector2> hookPoints = new List<Vector2>();
	// List<Vector2> hookCache1 = new List<Vector2>();
	// List<Vector2> hookCache2 = new List<Vector2>();

	Vector2 avgRodVelocity = Vector2.zero;
	float reelprogress = 1;

	Mouse mouse;
	Keyboard kbd;

	void Start() {
		// subdivide fishing line
		for (int i = 0; i < LinePointCount; i++) {
			hookPoints.Add(Vector3.up * 1 * .2f);
			// hookCache1.Add(Vector3.up * 1 * .2f);
			// hookCache2.Add(Vector3.up * 1 * .2f);
		}

		if (!ZoomCamera) ZoomCamera = Camera.main;

		mouse = Mouse.current;
		kbd = Keyboard.current;

	}

	private void FixedUpdate() {
		// make hook float upwards
		hookRigidbody.AddForce(Vector2.up * HookFloatRate, ForceMode2D.Force);

		// restrict max hook speed
		if (hookRigidbody.velocity.sqrMagnitude > HookVelocityCap * HookVelocityCap) {
			hookRigidbody.velocity = hookRigidbody.velocity.normalized * HookVelocityCap;
		}
		// hook.velocity = Vector2.zero;
	}

	bool reelIn = false;
	void Update() {



		float scroll = -mouse.scroll.y.ReadValue();
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

		Vector2 rodDeltaPos = RodEnd.position - rodEndOldPos;
		float rodDelta = rodDeltaPos.magnitude; //rodNewAngle - rodOldAngle;


		bool queuedSend = false;
		// if (kbd.spaceKey.isPressed) {
		//if (mouse.leftButton.isPressed) {
		if (mouse.leftButton.wasPressedThisFrame) {
			reelIn = true;
			FindObjectOfType<AudioManager>().Play("LineReel");
			//avgRodVelocity =Vector2.zero;
		} //else {
			if (mouse.leftButton.wasReleasedThisFrame)
			{
			// if (kbd.spaceKey.wasReleasedThisFrame) {
			reelIn = false;
			queuedSend = true;
			// hook.velocity = rodDeltaPos * LineWhipRate * Time.deltaTime;
		}

		if (reelIn) {
			reelprogress -= ReelSpeed * Time.deltaTime;
			// reelprogress = Mathf.Max(reelprogress, 0);

			if (Vector2.Distance(transform.position, hook.transform.position) < InstantReelThreshold) reelprogress = 0;

			if (reelprogress <= 0) hook.CatchFish();

			Vector2 vector2 = rodDeltaPos * LineWhipRate * Time.deltaTime;
			avgRodVelocity = (avgRodVelocity + vector2) * .5f;

		} else {
			reelprogress = 1;
		}

		// DebugText.SetText($"reel: {reelprogress.ToString("0.00")}, {reelIn}");
		float reelprogressLerp = Mathf.Lerp(ReeledMul, 1, ReelCurve.Evaluate(reelprogress));


		float minDistance = reelprogressLerp * LineMinPointDistance;
		float maxDistance = reelprogressLerp * LinePointDistance;


		// tug fishing line towards hook
		if (Vector3.Distance(hookRigidbody.position, hookPoints[hookPoints.Count - 1]) > maxDistance * .8f) {
			hookPoints[hookPoints.Count - 1] = hookRigidbody.position + (hookPoints[hookPoints.Count - 1] - hookRigidbody.position).normalized * maxDistance * .8f;
		}

		// tug other parts of line in order towards fishing rod
		for (int i = hookPoints.Count - 2; i >= 0; i--) {
			Vector3 prevPos = hookPoints[i + 1];
			Vector3 oldPos = hookPoints[i];
			if (Vector3.Distance(oldPos, prevPos) > maxDistance) {
				hookPoints[i] = prevPos + (oldPos - prevPos).normalized * maxDistance;
			}
		}

		// tug fishing line towards fishing rod
		hookPoints[0] = RodEnd.position;
		for (int i = 1; i < hookPoints.Count; i++) {

			if (i < hookPoints.Count - 1)
				hookPoints[i + 1] += Vector2.up * LineFloatRate * Time.deltaTime;

			// hookPoints[i] += Vector3.up * LineFloatRate * Time.deltaTime;

			Vector2 prevPos = hookPoints[i - 1];
			Vector2 oldPos = hookPoints[i];
			Vector2 oldDelta = oldPos - prevPos;
			float deltaDistance = Vector3.Distance(oldPos, prevPos);
			if (deltaDistance > maxDistance) {
				hookPoints[i] = prevPos + (oldPos - prevPos).normalized * maxDistance;
			} else if (deltaDistance < minDistance) {
				Vector2 normalizedDelta = (oldPos - prevPos).normalized;
				if (normalizedDelta == Vector2.zero) normalizedDelta = Vector3.one;
				hookPoints[i] = prevPos + normalizedDelta * minDistance;
			}

			// hookCache2[i] = hookPoints[i];

		}


		// enforce bending limit
		// for (int i = 2; i < hookPoints.Count; i++) {
		// 	Vector2 prevPos = hookPoints[i - 1];
		// 	Vector2 prePrevPos = hookPoints[i - 2];
		// 	Vector2 oldPos = hookPoints[i];
		// 	Vector2 prevDelta = prevPos - prePrevPos;
		// 	Vector2 oldDelta = oldPos - prevPos;
		// 	Vector2 cachedDelta1 = (hookCache1[i] - hookCache1[i - 1]);
		// 	Vector2 cachedDelta2 = (hookCache2[i] - hookCache2[i - 1]);
		// 	float cachedDistance = cachedDelta2.magnitude;

		// 	Vector2 newDelta = hookPoints[i] - prevPos;


		// 	float angleDiff = Vector2.SignedAngle(cachedDelta1, oldDelta);

		// 	if (Mathf.Abs(angleDiff) > MaxBendPerSec * Time.deltaTime) {
		// 		hookPoints[i] = prevPos + Rotate(cachedDelta1, Mathf.Sign(angleDiff) * MaxBendPerSec * Time.deltaTime).normalized * cachedDistance;
		// 	}
		// }

		// tug hook
		if (Vector3.Distance(hookRigidbody.position, hookPoints[hookPoints.Count - 1]) > maxDistance) {
			Vector2 delta = (hookRigidbody.position - hookPoints[hookPoints.Count - 1]);
			hookRigidbody.MovePosition(hookPoints[hookPoints.Count - 1] + delta.normalized * maxDistance);
			// flick hook if it was tugged
			// if (Vector3.Angle(hook.velocity, delta) < 45)
			// hook.velocity = Vector2.zero;

			// Vector2 newVelocity = -delta * Mathf.Abs(rodDelta) * LineWhipRate;
			// avgRodVelocity = (avgRodVelocity + newVelocity) / 2;
			// hook.AddForce(avgRodVelocity, ForceMode2D.Impulse);
		}

		if (queuedSend) {
			Vector2 vector2 = avgRodVelocity;//rodDeltaPos * LineWhipRate * Time.deltaTime;
			hookRigidbody.velocity = vector2;
			// Debug.Log($"sent hook with velocity {vector2} ({RodEnd.position}, {rodEndOldPos})");
		}


		// for (int i = 0; i < hookPoints.Count; i++) {
		// 	hookCache1[i] = hookPoints[i];
		// }

		// move player
		/*
		float x = 0;
		float y = 0;

		if (kbd.wKey.isPressed) {
			y += speed * Time.deltaTime;
			hook.AddForce(Vector2.up * 10);
		}
		if (kbd.aKey.isPressed) {
			x -= speed * Time.deltaTime;
		}
		if (kbd.sKey.isPressed) {
			y -= speed * Time.deltaTime;
		}
		if (kbd.dKey.isPressed) {
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
			// skips successive points in the same position
			if (Vector3.Distance(lastPos, item) > 1f) {
				line.spline.InsertPointAt(0, item);
				lastPos = item;
			}
		}
		if (Vector3.Distance(lastPos, hookRigidbody.position) > 1f) {
			line.spline.InsertPointAt(0, hookRigidbody.position);
		}

		// wobble player
		//float randomheight =Random.RandomRange(0.8f, 1.2f);
		// var scale = transform.localScale;
		// scale.y = randomheight;
		// transform.localScale = scale;

		rodEndOldPos = RodEnd.position;
		rodOldAngle = rodNewAngle;

	}

	public static Vector2 Rotate(Vector2 vector, float angle) {
		return Quaternion.Euler(0, 0, angle) * vector;
	}

}
