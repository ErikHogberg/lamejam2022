using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugText : MonoBehaviour {
	static DebugText mainInstance = null;

	public UnityEngine.UI.Text text;

	private void Awake() {
		mainInstance = this;
	}

	private void OnDestroy() {
		mainInstance = null;
	}


	public static void SetText(string newText) {
		if (!mainInstance || !mainInstance.text) {
			Debug.LogWarning("cant set text");
			return;
		}

		mainInstance.text.text = newText;
		// Debug.Log($"set text to {newText}");

	}

}
