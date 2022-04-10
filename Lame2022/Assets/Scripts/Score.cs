using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour {

	private static Score mainInstance = null;

	private int currentScore =0;
	public Text scoreText;

	private void Awake() {
		mainInstance = this;
	}

	public static void AddScore(int score) {
		if(!mainInstance) return;
		mainInstance.currentScore += score;
		mainInstance.scoreText.text = mainInstance.currentScore.ToString();
	}
}
